namespace PythonEngineSystem
{
    /// <summary>
    /// Provides an interface suitable for python engine.
    /// This interface will be used by python as stdout
    /// </summary>
    public interface IPythonLogger
    {
        void flush();
        void write(string str);
        void writeError(string str);
        void writelines(string[] str);
        void close();
        string ReadStream();

    } 
}
