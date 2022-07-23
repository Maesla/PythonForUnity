using UnityEngine;
using UnityEngine.InputSystem;

namespace MicrokernelSystem.UI
{
    public class PluginWindowController : MonoBehaviour, IPluginWindowController
    {
        [SerializeField]
        private InputAction enableWindowInputAction = default;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private bool autoHide;

        [SerializeField]
        private PluginWindow pluginWindow;

        private void Start()
        {
            enableWindowInputAction.Enable();
            enableWindowInputAction.started += InputEventHandler;

            if (autoHide)
            {
                SetVisible(false);
            }
        }

        private void InputEventHandler(InputAction.CallbackContext ctx)
        {
            ToggleVisibility();
        }

        public void ToggleVisibility()
        {
            SetVisible(!canvas.gameObject.activeSelf);
        }

        public void SetVisible(bool isVisible)
        {
            canvas.gameObject.SetActive(isVisible);
        }

        public void SetPlugin(IPlugin plugin)
        {
            pluginWindow.Set(plugin);
        }
    } 
}
