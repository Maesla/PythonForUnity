using MicrokernelSystem.UI;
using Zenject;

namespace MicrokernelSystem.Addons
{
    public class WindowProvider : BasePythonAPIProvider
    {
        private readonly LazyInject<IPluginWindowController> controller;

        public WindowProvider(LazyInject<IPluginWindowController> controller)
        {
            this.controller = controller;
        }

        public void Hide()
        {
            controller.Value.SetVisible(false);
        }
    } 
}
