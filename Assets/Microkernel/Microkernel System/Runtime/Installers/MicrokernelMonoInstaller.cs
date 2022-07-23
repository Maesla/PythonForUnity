using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MicrokernelSystem
{
    public class MicrokernelMonoInstaller : MonoInstaller<MicrokernelMonoInstaller>
    {
        [SerializeField]
        private string[] assembliesString;

        [SerializeField][FormerlySerializedAs("settingsRoot")]
        private string settingsFolderInStreamingAssets;

        private const string pathRootsParameterName = "pathRoots";

        public override void InstallBindings()
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, settingsFolderInStreamingAssets);
            //Workarround to use MicrokernelSettingsInstaller as an instance instead of a static call, so we can access to its data
            MicrokernelSettingsInstaller microkernelSettingsInstaller = 
                Container.InstantiateExplicit<MicrokernelSettingsInstaller>(InjectUtil.CreateArgListExplicit(fullPath));
            microkernelSettingsInstaller.InstallBindings();
            MicrokernelSystemSettings settings = microkernelSettingsInstaller.Settings;

            MicrokernelInstaller.Install(Container);
            InstallerFromAssembly<IAPIProvider>.Install(Container, assembliesString);
            InstallerFromAssembly<IRunnableContextFactory>.Install(Container, assembliesString);

            string [] pluginRoots = settings.GetValues<string>(pathRootsParameterName).ToArray();
            PluginExplorerInstaller.Install(Container, pluginRoots);

            Container.Bind(typeof(IInitializable), typeof(System.IDisposable)).To<MicrokernelSystemController>().AsSingle();
        }

        private void Reset()
        {
            assembliesString = new string[] { this.GetType().Assembly.GetName().Name };
        }
    }
}
