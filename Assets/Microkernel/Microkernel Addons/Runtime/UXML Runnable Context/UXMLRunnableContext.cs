namespace MicrokernelSystem.Addons
{
    public class UXMLRunnableContext : IRunnableContext
    {
        private readonly string code;

        public UXMLRunnableContext(string code)
        {
            this.code = code;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public string Execute()
        {
            return string.Empty;
        }
    }
}
