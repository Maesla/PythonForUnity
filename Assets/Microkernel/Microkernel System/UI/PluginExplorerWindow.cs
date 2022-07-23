using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MicrokernelSystem.UI
{
    public class PluginExplorerWindow : MonoBehaviour
    {
        [Inject]
        IPluginExplorer pluginExplorer;

        [SerializeField]
        private PluginTab pluginTab;

        [SerializeField]
        private PluginWindow pluginWindow;

        [SerializeField]
        private Text roots;

        private void OnEnable()
        {
            FillTabs();
            FillRoots();
            if (!pluginWindow.IsSet)
            {
                pluginWindow.Set(pluginExplorer.Plugins[0]);
            }
        }

        private void FillTabs()
        {
            foreach (var plugin in pluginExplorer.Plugins)
            {
                var newTab = GameObject.Instantiate(pluginTab, pluginTab.transform.parent);
                newTab.Init(plugin, pluginWindow);
            }

            pluginTab.gameObject.SetActive(false);
        }

        private void FillRoots()
        {
            roots.text = GetRootPaths();
        }

        private string GetRootPaths()
        {
            var roots = pluginExplorer.RootPaths.Select(s => $"• {s}");
            return string.Join("\n", roots);
        }
    } 
}
