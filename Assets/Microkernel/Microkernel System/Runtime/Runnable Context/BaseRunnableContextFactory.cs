using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MicrokernelSystem
{
    /// <summary>
    /// Base implementation for a IRunnableContextFactory. it gets all the available providers and filter them
    /// </summary>
    public abstract class BaseRunnableContextFactory : IRunnableContextFactory
    {
        public abstract string Type { get; }

        private readonly List<IAPIProvider> apiProviders;

        [Inject]
        public BaseRunnableContextFactory(List<IAPIProvider> apiProviders)
        {
            this.apiProviders = apiProviders;
        }

        public BaseRunnableContextFactory() : this(new List<IAPIProvider>()) { }

        public abstract IRunnableContext Create(PluginSettings settings);

        protected List<T> FilterProviders<T>(PluginSettings settings) where T : IAPIProvider
        {
            var providers =
                        apiProviders
                        .Where(p => p is T)
                        .Where(p => settings.version >= p.Version)
                        .Cast<T>()
                        .ToList();

            return providers;
        }

        public virtual void Initialize(){}

        public virtual void Finish(){}
    }
}
