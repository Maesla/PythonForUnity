using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MicrokernelSystem.UI
{
    internal class PluginWindow : MonoBehaviour
    {
        [SerializeField]
        private Text title;
        [SerializeField]
        private InputField code;

        [SerializeField]
        private Console console;

        [SerializeField]
        private Button runButton;

        [SerializeField]
        private Button openButton;

        [SerializeField]
        private Button reloadButton;

        [SerializeField]
        private Dropdown selectionDropdown;

        [Inject]
        private IMicrokernel microkernel;

        private IPlugin plugin;
        public bool IsSet { get; private set; }
        private ICode SelectedCode
        {
            get
            {
                return selectionDropdown.value == 0 ? plugin.Code : plugin.UI;
            }
        }

        private void Start()
        {
            runButton.onClick.AddListener(RunCode);
            selectionDropdown.onValueChanged.AddListener(SelectCode);
            openButton.onClick.AddListener(OpenCode);
            reloadButton.onClick.AddListener(ReloadCode);
        }

        private void RunCode()
        {
            PluginSettings settings = PluginSettings.Default();
            settings.type = selectionDropdown.captionText.text;
            settings.code = code.text;
            settings.id = plugin.Contract.Id;

            try
            {
                var run = microkernel.Execute(settings);;
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void SelectCode(int selection)
        {
            string c = selection == 0 ? plugin.Code.GetCode() : plugin.UI.GetCode();
            code.text = c;
        }

        private void OpenCode()
        {
            SelectedCode.OpenInDefaultApplication();
        }

        private void ReloadCode()
        {
            SelectedCode.Reload();
            SelectCode(selectionDropdown.value);
        }

        public void Set(IPlugin plugin)
        {
            this.plugin = plugin;
            title.text = plugin.Contract.Name;
            SelectCode(0);
            IsSet = true;
        }
    } 
}
