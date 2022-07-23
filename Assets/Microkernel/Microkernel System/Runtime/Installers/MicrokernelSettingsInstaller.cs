using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace MicrokernelSystem
{
    public class MicrokernelSettingsInstaller : Installer<string, MicrokernelSettingsInstaller>
    {
        private readonly string root;
        private const string settingsFilename = "MicrokernelSystemSettings.json";
        public MicrokernelSystemSettings Settings { private set; get; }

        public MicrokernelSettingsInstaller(string root)
        {
            this.root = root;
        }

        public override void InstallBindings()
        {
            InstallSettings();
        }

        private void InstallSettings()
        {

            try
            {
                string path = Path.Combine(root, settingsFilename);
                if (File.Exists(path))
                {
                    string settingsJson = File.ReadAllText(path);
                    Settings = new MicrokernelSystemSettings(settingsJson);
                }
                else
                {
                    Settings = MicrokernelSystemSettings.DefaultSettings;
                }

            }
            catch (Exception e)
            {
                Settings = MicrokernelSystemSettings.Empty;    
            }       
            Container.Bind<MicrokernelSystemSettings>().FromInstance(Settings).AsSingle();
        }
    } 
}
