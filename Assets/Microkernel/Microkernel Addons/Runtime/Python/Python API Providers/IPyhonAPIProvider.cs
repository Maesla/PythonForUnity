namespace MicrokernelSystem.Addons
{
    internal interface IPyhonAPIProvider : IAPIProvider
    {
        public string Code { get; }
        public PythonVariable[] Variables { get; }
    } 
}
