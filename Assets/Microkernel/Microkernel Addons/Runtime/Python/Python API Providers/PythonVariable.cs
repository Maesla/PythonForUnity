
namespace MicrokernelSystem.Addons
{
    public struct PythonVariable
    {
        public string name;
        public object obj;

        public PythonVariable(string name, object obj)
        {
            this.name = name;
            this.obj = obj;
        }
    } 
}
