using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MicrokernelSystem
{
    public class Microkernel : IMicrokernel
    {
        private readonly List<IRunnableContextFactory> runnableFactories;

        /// <summary>
        /// This buffer is passed throught all the microkernel system. The code could use the buffer to write in it
        /// </summary>
        private readonly Buffer runBuffer;

        /// <summary>
        /// Stores a registry about all the runs
        /// </summary>
        private readonly IRegistry registry;

        public List<IRunnableContextFactory> RunnableFactories => runnableFactories;

        public bool Initialized { get; set; }

        public bool InitializationFailed { get; set; }

        [Inject]
        public Microkernel(List<IRunnableContextFactory> runnableFactories, Buffer runBuffer, IRegistry registry)
        {
            this.runnableFactories = runnableFactories;
            this.runBuffer = runBuffer;
            this.registry = registry;
        }

        public Microkernel() : this(new List<IRunnableContextFactory>(), null, null) { }

        public Microkernel(params IRunnableContextFactory[] factories) : this(factories.ToList(), null, null) { }

        public IRunnableContext Create(PluginSettings settings)
        {
            try
            {
                IRunnableContextFactory factory = runnableFactories.First(factory => settings.type == factory.Type);
                IRunnableContext runnableContext = factory.Create(settings);
                return runnableContext;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Sequence"))
            {
                string msg = $"There is not a runnable context available for {settings.type}";
                throw new ArgumentException(msg, ex);
            }
        }

        /// <summary>
        /// The main method of the system. It runs the code, and creates a new entry in the registry, with the result, and the buffer content
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>returns a class with relevant information about the run</returns>
        public Run Execute(PluginSettings settings)
        {
            runBuffer?.Open();

            var runnableContext = Create(settings);
            var result = runnableContext.Execute();

            object[] bufferResult = runBuffer != null ? runBuffer.Close() : Array.Empty<object>();

            Run newRun = new Run(runnableContext, result, settings, bufferResult);
            registry.Add(newRun);

            return new Run(runnableContext, result, settings, bufferResult);
        }

        public void Initialize()
        {
            try
            {
                foreach (var factory in runnableFactories)
                {
                    factory.Initialize();
                }

                Initialized = true;
                InitializationFailed = false;
            }
            catch(Exception e)
            {
                Initialized = false;
                InitializationFailed = true;
                throw new InvalidOperationException("Python Install could not be found", e);
            }
        }

        public void Finish()
        {
            foreach (Run run in registry)
            {
                run.runnableContext.Dispose();
            }

            foreach (var factory in runnableFactories)
            {
                factory.Finish();
            }
            
            Initialized = false;
        }
    }
}
