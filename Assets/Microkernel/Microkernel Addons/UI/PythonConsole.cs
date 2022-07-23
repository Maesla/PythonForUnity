using MicrokernelSystem.UI;
using PythonEngineSystem;
using Zenject;

namespace MicrokernelSystem.Addons.UI
{
    public class PythonConsole : IPythonLogger
    {
        private readonly IPythonLogger decoratedLogger;

        private readonly LazyInject<Console> lazyConsole;
        private Console Console => lazyConsole.Value;

        public PythonConsole(IPythonLogger decoratedLogger, LazyInject<Console> console)
        {
            this.decoratedLogger = decoratedLogger;
            this.lazyConsole = console;
        }

        public void close()
        {
            decoratedLogger.close();
        }

        public void flush()
        {
            Console.Log(string.Empty);
            decoratedLogger.flush();
        }

        public string ReadStream()
        {
            return decoratedLogger.ReadStream();
        }

        public void write(string str)
        {
            decoratedLogger.write(str);
            Console.Log(decoratedLogger.ReadStream());
        }

        public void writeError(string str)
        {
            Console.Log(decoratedLogger.ReadStream());

            decoratedLogger.writeError(str);
            Console.LogError(str);
        }

        public void writelines(string[] str)
        {
            decoratedLogger.writelines(str);
        }
    }
}
