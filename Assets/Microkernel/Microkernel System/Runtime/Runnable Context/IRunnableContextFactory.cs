namespace MicrokernelSystem
{
    /// <summary>
    /// Creates a runnable context suitable for a spefic plugin
    /// </summary>
    public interface IRunnableContextFactory
    {
        string Type { get; }
        void Initialize();
        IRunnableContext Create(PluginSettings settings);
        void Finish();
    } 
}
