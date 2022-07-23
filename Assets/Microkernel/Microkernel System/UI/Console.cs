using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MicrokernelSystem.UI
{
    public class Console : MonoBehaviour
    {
        [SerializeField]
        private Text console;

        private ConsoleFormatter formatter;

        private void Awake()
        {
            formatter = new ConsoleFormatter();
        }

        private void Start()
        {
            Log(string.Empty);
        }

        public void Log(string output)
        {
            console.text = formatter.Format(output);
        }

        public void LogError(string output)
        {
            console.text = formatter.FormatError(output);
        }
    }
}