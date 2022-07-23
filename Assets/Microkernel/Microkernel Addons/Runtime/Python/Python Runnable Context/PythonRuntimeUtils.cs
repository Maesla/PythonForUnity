using System;

namespace MicrokernelSystem.Addons
{
    public static class PythonRuntimeUtils
    {

        public static readonly string EnvironmentVariable = "PYTHONNET_PYDLL";

        public static bool IsEnvironmentVariableDefined()
        {
            string dllPath = Environment.GetEnvironmentVariable(EnvironmentVariable);
            return !string.IsNullOrEmpty(dllPath);
        }

    } 
}
