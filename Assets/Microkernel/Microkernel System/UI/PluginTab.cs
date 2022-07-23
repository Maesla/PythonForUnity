using UnityEngine;
using UnityEngine.UI;

namespace MicrokernelSystem.UI
{
    internal class PluginTab : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Text label;


        private IPlugin plugin;
        private PluginWindow pluginWindow;

        private void Awake()
        {
            button.onClick.AddListener(SetupWindow);
        }

        public void Init(IPlugin plugin, PluginWindow window)
        {
            this.pluginWindow = window;
            this.plugin = plugin;
            this.plugin = plugin;
            label.text = plugin.Contract.Name;
        }

        private void SetupWindow()
        {
            pluginWindow.Set(plugin);
        }
    } 
}
