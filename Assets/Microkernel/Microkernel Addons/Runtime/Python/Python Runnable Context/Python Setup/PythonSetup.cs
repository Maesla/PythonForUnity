using Python.Runtime;
using System;
using System.IO;

namespace MicrokernelSystem.Addons
{
    public class PythonSetup : IPythonSetup
    {
        internal readonly IPythonEngineSettings settings;
        private bool done;

        public PythonSetup() { }

        public PythonSetup(IPythonEngineSettings settings)
        {
            this.settings = settings;
        }

        public void Setup()
        {
            if (done)
            {
                return;
            }

            if (settings != null)
            {
                string path = settings.DLLPath;
                if (File.Exists(path))
                {
                    Runtime.PythonDLL = path;
                    done = true;
                    return;
                }

                throw new InvalidOperationException("Python Install could not be found");

            }

            throw new InvalidOperationException("Python Install could not be found");
        }
    }
}
