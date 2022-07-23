namespace MicrokernelSystem
{
    public interface IMicrokernel
    {
        void Initialize();
        
        /// <summary>
        /// Creates a Runnable Context. This context can be run
        /// </summary>
        /// <param name="settings"></param>
        IRunnableContext Create(PluginSettings settings);
        
        /// <summary>
        /// Executes a pluging
        /// </summary>
        /// <param name="settings">the necessary data to run the plugin</param>
        /// <returns>returns a class with relevant information about the run</returns>
        Run Execute(PluginSettings settings);
        void Finish();

        bool Initialized { get; }
        bool InitializationFailed { get; }
    }
}
