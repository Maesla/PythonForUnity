using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MicrokernelSystem.UI
{
    public class RegistryWindow : MonoBehaviour
    {
        [Inject]
        private IRegistry registry;

        [SerializeField]
        private Text runsTextField;

        private void OnEnable()
        {
            registry.OnRunAdded += RunAddedHandler;
            UpdateList();
        }

        private void OnDisable()
        {
            registry.OnRunAdded -= RunAddedHandler;
        }

        private void RunAddedHandler(Run newRun)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            var runs = registry.Select(run => $"• {run}");
            runsTextField.text = string.Join("\n", runs);
        }
    } 
}
