using Zenject;

namespace MicrokernelSystem
{
    public class PluginExplorerInstaller : Installer<string[], PluginExplorerInstaller>
    {
        private readonly string[] roots;

        public PluginExplorerInstaller(string[] root)
        {
            this.roots = root;
        }

        public override void InstallBindings()
        {
            InstallPluginExplorer();
        }

        private void InstallPluginExplorer()
        {
            PluginExplorer pluginExplorer = new PluginExplorer
            {
                RootPaths = roots
            };

            pluginExplorer.Explore();
            Container.Bind<IPluginExplorer>().FromInstance(pluginExplorer);
        }
    }
}