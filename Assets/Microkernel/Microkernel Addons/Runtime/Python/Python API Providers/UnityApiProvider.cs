using System;

namespace MicrokernelSystem.Addons
{
    internal class UnityApiProvider : IPyhonAPIProvider
    {
        public string[] Types => new string[] { "Python" };

        public SemanticVersioning Version => new SemanticVersioning("0.0.0");

        public string Code
        {
            get
            {
                return "from UnityEngine import *";
            }
        }

        public PythonVariable[] Variables => Array.Empty<PythonVariable>();
    }
}
