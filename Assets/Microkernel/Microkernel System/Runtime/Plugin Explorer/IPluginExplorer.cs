namespace MicrokernelSystem
{
    /// <summary>
    /// Explores all the RootPaths and creates an array of valid IPlugins found on RoothPaths
    /// </summary>
    public interface IPluginExplorer
    {
        IPlugin[] Plugins { get; }
        void Explore();
        string[] RootPaths { get; }
    }
}
