namespace MicrokernelSystem.UI
{
    public interface IPluginWindowController
    {
        void SetVisible(bool isVisible);
        void ToggleVisibility();

        void SetPlugin(IPlugin plugin);
    }
}
