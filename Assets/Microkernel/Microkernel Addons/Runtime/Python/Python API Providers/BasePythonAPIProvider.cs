namespace MicrokernelSystem.Addons
{
    public abstract class BasePythonAPIProvider : IPyhonAPIProvider
    {
        public virtual string Code => string.Empty;

        public string[] Types => new string[] { "Python" };

        public SemanticVersioning Version => new SemanticVersioning("0.0.0");

        public virtual PythonVariable[] Variables => new PythonVariable[] { new PythonVariable { name = GetCamelCaseName(), obj = this } };

        protected string GetCamelCaseName()
        {
            string name = this.GetType().Name;
            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}
