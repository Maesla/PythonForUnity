using System.IO;

namespace MicrokernelSystem
{
    public class Code : ICode
    {
        private string code;

        public string Path { get; private set; }

        public Code(string path)
        {
            Path = path;
            Load();
        }

        public void SetCode(string code)
        {
            this.code = code;
        }

        public string GetCode()
        {
            return code;
        }

        public void OpenInDefaultApplication()
        {
            System.Diagnostics.Process.Start(Path);
        }

        public void Reload()
        {
            Load();
        }

        private void Load()
        {
            if (File.Exists(Path))
            {
                string code = File.ReadAllText(Path);
                SetCode(code);
            }
        }
    } 
}
