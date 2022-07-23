using PythonEngineSystem;
using System.Collections.Generic;
using Zenject;

namespace MicrokernelSystem.Addons
{
    /// <summary>
    /// It creates and prepares a PythonRunnableContext
    /// </summary>
    internal class PythonRunnableContextFactory : BaseRunnableContextFactory
    {
        private readonly IPythonSetup setup;
        private readonly IPythonLogger logger;

        public PythonRunnableContextFactory() : this(new List<IAPIProvider>()) { }
        public PythonRunnableContextFactory(List<IAPIProvider> apiProviders) : this(apiProviders, null, new PythonLogger()){ }
        
        [Inject]
        public PythonRunnableContextFactory(List<IAPIProvider> apiProviders, IPythonSetup setup, IPythonLogger logger): base(apiProviders)
        {
            this.setup = setup;
            this.logger = logger;
        }


        public override string Type => "Python";

        public override IRunnableContext Create(PluginSettings settings)
        {
            var providers = FilterProviders<IPyhonAPIProvider>(settings);
            var runnableContext = new PythonRunnableContext(providers, logger, settings.code);
            runnableContext.Initialize();
            return runnableContext;
        }

        public override void Initialize()
        {
            setup?.Setup();
            PythonRunnableContext.Startup();

        }

        public override void Finish()
        {
            PythonRunnableContext.Shutdown();
        }
    }
}
