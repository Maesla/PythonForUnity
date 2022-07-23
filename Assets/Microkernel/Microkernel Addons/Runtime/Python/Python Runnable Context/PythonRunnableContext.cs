using PythonEngineSystem;
using System.Collections.Generic;
using Zenject;

namespace MicrokernelSystem.Addons
{
    /// <summary>
    /// This class creates a context able to run python code
    /// </summary>
    internal class PythonRunnableContext : IRunnableContext
    {
        private readonly PythonEngine engine;
        internal List<IPyhonAPIProvider> apiProviders;
        private readonly string code;

        public PythonRunnableContext():this(new List<IPyhonAPIProvider>(), new PythonLogger(), string.Empty) {}

        public PythonRunnableContext(List<IPyhonAPIProvider> apiProviders) : this(apiProviders, new PythonLogger(), string.Empty) { }

        [Inject]
        public PythonRunnableContext(List<IPyhonAPIProvider> apiProviders, IPythonLogger logger, string code)
        {
            this.apiProviders = apiProviders;

            engine = new PythonEngine(logger);
            this.code = code;
        }

        public static void Startup()
        {
            PythonEngine.Startup();
        }

        public void Initialize()
        {
            engine.Initialize();

            foreach (var provider in apiProviders)
            {
                if (!string.IsNullOrEmpty(provider.Code))
                {
                    engine.Execute(provider.Code);
                }

                foreach (PythonVariable variable in provider.Variables)
                {
                    engine.SetVariable(variable.name, variable.obj);
                }
            }
        }

        public string Execute()
        {
            return Execute(code);
        }

        public string Execute(string code)
        {
            return engine.Execute(code);
        }

        public void SetVariable(string name, object value)
        {
            engine.SetVariable(name, value);
        }

        public T GetVariable<T>(string name)
        {
            return engine.GetVariable<T>(name);
        }

        public void Dispose()
        {
            Finish();
        }

        public void Finish()
        {
            engine.Finish();
        }

        public static void Shutdown()
        {
            PythonEngine.Shutdown();
        }
    } 
}
