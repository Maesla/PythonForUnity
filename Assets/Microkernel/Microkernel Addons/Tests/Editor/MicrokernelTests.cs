using NSubstitute;
using NUnit.Framework;
using PythonEngineSystem;
using System.Collections.Generic;
using System.IO;
using Zenject;

namespace MicrokernelSystem.Addons.Editor.Tests
{
    public class MicrokernelTests
    {
        [Test]
        public void CreationTest()
        {
            PythonRunnableContextFactory pythonFactory = new PythonRunnableContextFactory();
            Microkernel microkernel = new Microkernel(new List<IRunnableContextFactory>() { pythonFactory }, null, new Registry());
            microkernel.Initialize();
            PluginSettings settings = PluginSettings.Default();
            IRunnableContext runnableContext = microkernel.Create(settings);

            Assert.That(runnableContext, Is.InstanceOf<PythonRunnableContext>());
            microkernel.Finish();
        }

        [Test]
        public void ApiTest()
        {
            DiContainer container = new DiContainer();
            MicrokernelInstaller.Install(container);
            IPyhonAPIProvider pythonApi = Substitute.For<IPyhonAPIProvider>();
            pythonApi.Version.Returns(new SemanticVersioning());
            container.BindInterfacesTo<IPyhonAPIProvider>().FromInstance(pythonApi).AsTransient();

            IPythonSetup setup = Substitute.For<IPythonSetup>();
            container.Bind<IPythonSetup>().FromInstance(setup);

            IPythonLogger logger = Substitute.For<IPythonLogger>();
            container.Bind<IPythonLogger>().FromInstance(logger);

            container.Bind<IRunnableContextFactory>().To<PythonRunnableContextFactory>().AsTransient();

            var iMicrokernel = container.Resolve<IMicrokernel>();
            iMicrokernel.Initialize();
            Assert.That(iMicrokernel, Is.InstanceOf<Microkernel>());

            var microkernel = iMicrokernel as Microkernel;
            CollectionAssert.IsNotEmpty(microkernel.RunnableFactories);

            PluginSettings settings = new PluginSettings { type = "Python", version = new SemanticVersioning() };
            IRunnableContext irunnableContext = microkernel.Create(settings);
            Assert.That(irunnableContext, Is.InstanceOf<PythonRunnableContext>());
            var pythonRunnableContext = irunnableContext as PythonRunnableContext;

            CollectionAssert.IsNotEmpty(pythonRunnableContext.apiProviders);

            iMicrokernel.Finish();

        }

        [Test]
        public void ExecutionTest()
        {
            PythonRunnableContextFactory pythonRunnableContextFactory = new PythonRunnableContextFactory();
            Microkernel microkernel = new Microkernel(new List<IRunnableContextFactory> { pythonRunnableContextFactory }, null, new Registry());
            microkernel.Initialize();
            string code = "a = 1\nb=a+1"; // a = 1; b = 2
            PluginSettings settings = new PluginSettings { type = "Python", version = new SemanticVersioning(), code = code };
            PythonRunnableContext pythonRunnableContext = microkernel.Create(settings) as PythonRunnableContext;
            using (pythonRunnableContext)
            {
                pythonRunnableContext.Execute();
                int a = pythonRunnableContext.GetVariable<int>("a");
                int b = pythonRunnableContext.GetVariable<int>("b");
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
            }
            microkernel.Finish();
        }

        [Test]
        public void PythonSettingsTest()
        {
            DiContainer container = new DiContainer();
            string pluginPath = GetPluginPath();
            string[] roots = new string[] { GetPluginPath() };

            PluginExplorerInstaller.Install(container, roots);
            MicrokernelSettingsInstaller.Install(container, pluginPath);

            MicrokernelSystemSettings settings = container.Resolve<MicrokernelSystemSettings>();
            PythonInstaller.Install(container, settings);

            PythonSetup setup = container.Resolve<IPythonSetup>() as PythonSetup;

            Assert.That(setup, Is.Not.Null);
            Assert.That(setup.settings, Is.Not.Null);
            Assert.That(setup.settings.DLLPath, Is.Not.Empty);
        }

        [Test]
        public void MicroKernelInitialization()
        {
            DiContainer container = new DiContainer();
            string pluginPath = GetPluginPath();
            MicrokernelSettingsInstaller.Install(container, pluginPath);
            MicrokernelSystemSettings settings = container.Resolve<MicrokernelSystemSettings>();
            PythonInstaller.Install(container, settings);

            PythonSetup setup = container.Resolve<IPythonSetup>() as PythonSetup;
            PythonRunnableContextFactory pythonRunnableContextFactory = new PythonRunnableContextFactory(new List<IAPIProvider>(), setup, null);
            Microkernel microkernel = new Microkernel(new List<IRunnableContextFactory> { pythonRunnableContextFactory }, null, new Registry());
            
            microkernel.Initialize();

            Assert.That(microkernel.Initialized, Is.True);
            Assert.That(microkernel.InitializationFailed, Is.False);

            microkernel.Finish();
        }

        [Test]
        public void MicroKernelInitializationFailed()
        {
            IPythonEngineSettings settings = Substitute.For<IPythonEngineSettings>();
            settings.DLLPath.Returns("anyDLL");
            PythonSetup setup = new PythonSetup(settings);
            PythonRunnableContextFactory pythonRunnableContextFactory = new PythonRunnableContextFactory(new List<IAPIProvider>(), setup, null);
            Microkernel microkernel = new Microkernel(new List<IRunnableContextFactory> { pythonRunnableContextFactory }, null, new Registry());

            Assert.Catch(microkernel.Initialize);
            Assert.That(microkernel.Initialized, Is.False);
            Assert.That(microkernel.InitializationFailed, Is.True);
            microkernel.Finish();
        }

        [Test]
        public void InstallerTests()
        {
            string[] assemblies = new[] { "MicrokernelSystem", "MicrokernelSystem.Addons" };

            string pluginPath = GetPluginPath();
            string[] roots = new string[] { pluginPath};
            DiContainer container = new DiContainer();
            MicrokernelInstaller.Install(container);
            MicrokernelSettingsInstaller.Install(container, pluginPath);
            InstallerFromAssembly<IAPIProvider>.Install(container, assemblies);
            InstallerFromAssembly<IRunnableContextFactory>.Install(container, assemblies);
            PluginExplorerInstaller.Install(container, roots);

            MicrokernelSystemSettings settings = container.Resolve<MicrokernelSystemSettings>();
            PythonInstaller.Install(container, settings);

            IMicrokernel kernel = container.Resolve<IMicrokernel>();
            kernel.Initialize();
            PluginSettings pluginSettings = PluginSettings.Default();
            Assert.DoesNotThrow(() => kernel.Create(pluginSettings));
            kernel.Finish();
        }


        [Test]
        public void ProviderWithDependenciesTest()
        {
            string[] assemblies = new[] { "MicrokernelSystem", "MicrokernelSystem.Addons", typeof(Bar).Assembly.GetName().Name };

            string pluginPath = GetPluginPath();
            string[] roots = new string[] { pluginPath };
            DiContainer container = new DiContainer();
            // unfortunally, we need to bind this int to satisfy CallbackProvider from other test :(
            container.Bind<int>().FromInstance(-1);

            MicrokernelInstaller.Install(container);
            MicrokernelSettingsInstaller.Install(container, pluginPath);
            InstallerFromAssembly<IAPIProvider>.Install(container, assemblies);
            InstallerFromAssembly<IRunnableContextFactory>.Install(container, assemblies);
            PluginExplorerInstaller.Install(container, roots);

            MicrokernelSystemSettings settings = container.Resolve<MicrokernelSystemSettings>();
            PythonInstaller.Install(container, settings);

            IMicrokernel kernel = container.Resolve<IMicrokernel>();
            Assert.Throws<ZenjectException>(() => container.Resolve<Bar>());
        }

        public class Foo : BasePythonAPIProvider { }

        public class Bar : BasePythonAPIProvider
        {
            private readonly Foo foo;

            public Bar(Foo foo)
            {
                this.foo = foo;
            }
        }


        private static string GetPluginPath()
        {
            string fileName = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            string rootPath = Path.GetDirectoryName(fileName);
            rootPath = Path.Combine(rootPath, "Plugins");
            return rootPath;
        }
    }
}