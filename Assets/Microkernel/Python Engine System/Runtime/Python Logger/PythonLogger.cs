using System.Collections.Generic;
using System.Diagnostics;

namespace PythonEngineSystem
{
    public class PythonLogger : IPythonLogger
    {
        private readonly List<string> buffer;

        public PythonLogger()
        {
            buffer = new List<string>();
        }

        public void close()
        {
            buffer.Clear();
        }

        public void flush()
        {
            buffer.Clear();
        }

        public string ReadStream()
        {
            var str = string.Join("\n", buffer);
            return str;
        }

        public void write(string str)
        {
            if (str == "\n")
            {
                return;
            }

            buffer.Add(str);
            Trace.WriteLine(str);
        }

        public void writeError(string str)
        {
            write(str);
        }

        public void writelines(string[] str)
        {
            buffer.AddRange(str);
        }
    }
}