using MicrokernelSystem;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PluginExplorerDebug", menuName = "Installers/PluginExplorerDebug")]
internal class PluginExplorerInstallerDebug : ScriptableObjectInstaller<PluginExplorerInstallerDebug>, IPluginExplorer
{
    public Plugin[] plugins;

    public IPlugin[] Plugins
    {
        get
        {
            return plugins;
        }
    }

    public void Explore()
    {
    }

    public override void InstallBindings()
    {
        Container.Bind<IPluginExplorer>().FromInstance(this);
    }

    public string[] RootPaths => Array.Empty<string>();


    [Serializable]
    internal class Plugin : IPlugin, IContract, ICode, IICon
    {
        public string name;
        public string id;
        [Multiline]
        public string code;

        public IContract Contract => this;

        public ICode Code => this;

        public ICode UI => this;

        public IICon Icon => this;

        public string Name => name;
        public string Id => id;

        public string Path => throw new NotImplementedException();

        public Texture2D Texture => throw new NotImplementedException();

        public Sprite Sprite => throw new NotImplementedException();

        public string Author => throw new NotImplementedException();

        public SemanticVersioning Version => throw new NotImplementedException();

        public string GetCode()
        {
            return code;
        }

        public void SetCode(string code)
        {
            this.code = code;
        }

        public void OpenInDefaultApplication()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}