using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PythonRuntineEngine = Python.Runtime.PythonEngine;

namespace PythonEngineSystem
{
    public class PythonEngine : IPythonEngine
    {
        private readonly IPythonLogger logger;
        private PyScope scope;

        private static bool initialized;
        private static IntPtr threadState;

        public PythonEngine(IPythonLogger logger)
        {
            this.logger = logger;
        }

        public static void Startup()
        {
            if (!initialized)
            {
                // Soft mode prevents Unity to crash in the second run
                //https://github.com/pythonnet/pythonnet/pull/958
                PythonRuntineEngine.Initialize(mode: ShutdownMode.Soft);
                threadState = PythonRuntineEngine.BeginAllowThreads();

                initialized = true;
            }
        }

        public static void Shutdown()
        {
            if (initialized)
            {
                PythonRuntineEngine.EndAllowThreads(threadState);
                PythonRuntineEngine.Shutdown(mode: ShutdownMode.Soft);
                initialized = false;
            }
        }

        public void Initialize()
        {
            using (Py.GIL())
            {
                scope = Py.CreateScope();
                SetupLogger();
            }
        }

        public void Finish()
        {
            Dispose();
        }

        public void Dispose()
        {
            using (Py.GIL())
            {
                if (initialized)
                {
                    scope.Dispose();
                }
                else
                {
                    scope = null;
                }
            }
        }

        [Obsolete("This method is highly experimental. Use carefully")]
        public void ExecuteInThread(string code)
        {
            void LocalExecute()
            {
                using (Py.GIL())
                {
                    PyScope ps = scope.NewScope();   
                    var pyCompile = PythonRuntineEngine.Compile(code);
                    ps.Execute(pyCompile);
                }
            }

            var asyncCall = Task.Factory.StartNew(() => LocalExecute());
        }

        public string Execute(string code)
        {
            return Execute(code, scope, logger);
        }

        private string Execute(string code, PyScope scope, IPythonLogger logger)
        {
            if (!initialized)
            {
                string msg = $"Engine has not been initialized. Please startup the engine";
                throw new InvalidOperationException(msg);
            }

            string result;
            logger.flush();
            try
            {
                using (Py.GIL())
                {
                    var pyCompile = PythonRuntineEngine.Compile(code);
                    scope.Execute(pyCompile);
                    result = logger.ReadStream();
                }
            }
            catch (PythonException ex)
            {
                string msg = $"{ex.Message}\n{ex.StackTrace}";
                logger.writeError(msg);
                throw;
            }
            catch (Exception ex)
            {
                result = $"Trace: \n{ex.StackTrace} " + "\n" +
                    $"Message: \n {ex.Message}" + "\n";
            }

            return result;
        }


        public IList<string>SearchPaths()
        {
            var pythonPaths = new List<string>();
#if NET_4_6
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                foreach (PyObject path in sys.path)
                {
                    pythonPaths.Add(path.ToString());
                }
            }
#endif

            return pythonPaths.Select(Path.GetFullPath).ToList();
        }

        public void SetSearchPath(IList<string> paths)
        {
            var searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                var src = "import sys\n" +
                           $"sys.path.extend({searchPaths.ToPython()})";
                Execute(src);
            }
        }

        public void SetVariable(string name, object value)
        {
            using (Py.GIL())
            {
                scope.Set(name, value.ToPython());
            }
        }

        public T GetVariable<T>(string name)
        {
            using (Py.GIL())
            {
                return scope.Get<T>(name);
            }
        }

        private void SetupLogger()
        {
            SetVariable("Logger", logger);
            const string loggerSrc = "import sys\n" +
                                        "from io import StringIO\n" +
                                        "sys.stdout = Logger\n" +
                                        "sys.stdout.flush()\n" +
                                        "sys.stderr = Logger\n" +
                                        "sys.stderr.flush()\n";
            Execute(loggerSrc);
        }
    }
}
