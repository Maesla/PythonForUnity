using System;

namespace MicrokernelSystem.Addons
{
    /// <summary>
    /// Configuration class for the python engine
    /// </summary>
    [Serializable]
    public class PythonEngineSettings : IPythonEngineSettings
    {
        /// <summary>
        /// Python dll path. It targets the local python installation.
        /// </summary>
        /// <example>"C:\\Program Files\\Python\\Python39\\python39.dll"</example>
        public string dllPath;
        public string DLLPath { get => dllPath; }

        public PythonEngineSettings(string dllPath)
        {
            this.dllPath = dllPath;
        }

    }
}
