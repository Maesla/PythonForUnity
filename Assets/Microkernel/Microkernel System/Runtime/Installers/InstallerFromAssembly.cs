using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MicrokernelSystem
{
    /// <summary>
    /// Installs all the classes inheritated from TMainType cointaned in the provided assembly
    /// </summary>
    /// <remarks>The classes will be created in a subcontainer and TMainType will be exposed in the supercontainer</remarks>
    /// <example>IFoo is implemented by Foo0, Foo1, FooN. A subcontainer will be created and Foo0..FooN will be injectd,
    /// But the supercontainer will be allowed only to access to the interface, not the implementation.
    /// [Inject]Foo0 => Valid in the subcontainer, invalid outside
    /// [Inject]List<IFoo> foos => valid outside
    /// </example>
    public class InstallerFromAssembly<TMainType> : Installer<IEnumerable<string>, InstallerFromAssembly<TMainType>>
    {
        private readonly IEnumerable<string> assembliesString;

        public InstallerFromAssembly(IEnumerable<string> assemblies)
        {
            this.assembliesString = assemblies;
        }

        public override void InstallBindings()
        {
            IEnumerable<Assembly> assemblies =
                assembliesString.Select(s => Assembly.Load(s));

            Action<ConventionSelectTypesBinder> generator = CreateTypesGenerator(assemblies);

            DiContainer subcontainer = BindTypesInSubContainer(assemblies, generator);
            ExposeMainTypeResolvingFromSubContainer(subcontainer);
        }

        private Action<ConventionSelectTypesBinder> CreateTypesGenerator(IEnumerable<Assembly> assemblies)
        {
            return x =>
                x
                .AllNonAbstractClasses()
                .DerivingFrom<TMainType>()
                .FromAssemblies(assemblies);
        }

        /// <summary>
        /// Binds all the interfaces and classes in a subcontainer, 
        /// so the instances can be inyected with other instances, but these instances are not exposed to the main Container
        /// </summary>
        private DiContainer BindTypesInSubContainer(IEnumerable<Assembly> assemblies, Action<ConventionSelectTypesBinder> generator)
        {
            var subcontainer = Container.CreateSubContainer();

            ConventionBindInfo bindInfo = new ConventionBindInfo();
            bindInfo.AddTypeFilter(t => t.IsInterface || (t.IsClass && !t.IsAbstract));
            bindInfo.AddAssemblyFilter(assembly => assemblies.Contains(assembly));
            var bindingTypes = bindInfo.ResolveTypes();
            
            subcontainer
                .Bind(bindingTypes)
                .To(generator)
                .AsSingle();
            
            return subcontainer;
        }

        /// <summary>
        /// Exposes the main type, looking up into the subcontainer
        /// </summary>
        /// <param name="subcontainer"></param>
        private void ExposeMainTypeResolvingFromSubContainer(DiContainer subcontainer)
        {
            Container.Bind(typeof(TMainType))
                .FromSubContainerResolveAll()
                .ByInstance(subcontainer)
                .AsSingle();
        }
    }
}
