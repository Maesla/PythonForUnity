using NSubstitute;
using NUnit.Framework;
using PythonEngineSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MicrokernelSystem.Addons.Editor.Tests
{
    public class RunTests
    {
        private Microkernel microkernel;
        private PluginSettings settings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            PythonRunnableContextFactory pythonRunnableContextFactory = new PythonRunnableContextFactory();
            List<IRunnableContextFactory> factories = new List<IRunnableContextFactory>() { pythonRunnableContextFactory };
            microkernel = new Microkernel(factories, null, new Registry());
            microkernel.Initialize();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            microkernel.Finish();
        }

        [Test]
        public void ResultTest()
        {
            string code = "print(\"a\")\nprint(\"b\")";
            var run = RunCode(code);
            Assert.That(run.result, Is.EqualTo("a\nb"));
        }

        [Test]
        public void TimestampTest()
        {

            var run = RunCode(string.Empty);
            Assert.That(run.timestamp, Is.EqualTo(DateTime.Now));
        }

        [Test]
        public void TypeTest()
        {
            var run = RunCode(string.Empty);
            Assert.That(run.settings.type, Is.EqualTo(settings.type));
        }

        [Test]
        public void PluginId()
        {
            var run = RunCode(string.Empty);
            Assert.That(run.settings.id, Is.EqualTo(settings.id));
        }


        private Run RunCode(string code)
        {
            settings = Create(code);
            return microkernel.Execute(settings);

        }

        private PluginSettings Create(string code)
        {
            return new PluginSettings { type = "Python", version = new SemanticVersioning(), code = code, id = "1234" };
        }

        [Test]
        public void BufferTest()
        {
            DiContainer container = new DiContainer();
            MicrokernelInstaller.Install(container);
            container.Bind<IAPIProvider>().To<BufferUser>().AsSingle();
            container.Bind<IPythonSetup>().FromInstance(null);
            IPythonLogger logger = Substitute.For<IPythonLogger>();
            container.Bind<IPythonLogger>().FromInstance(logger);
            container.Bind<IRunnableContextFactory>().To<PythonRunnableContextFactory>().AsSingle();

            IMicrokernel kernel = container.Resolve<IMicrokernel>();
            int countToWrite = 10;
            PluginSettings s = Create($"bufferUser.Write({countToWrite})");
            var run = kernel.Execute(s);
            Assert.That(run.objs.Length, Is.EqualTo(countToWrite));
            int[] actual = run.objs.Cast<int>().ToArray();
            int[] expected = Enumerable.Range(0, 10).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        private class BufferUser : BasePythonAPIProvider
        {
            private readonly IBufferWriter buffer;

            public BufferUser(IBufferWriter buffer)
            {
                this.buffer = buffer;
            }

            public void Write(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    buffer.Write(i);
                }
            }
        }
    } 
}
