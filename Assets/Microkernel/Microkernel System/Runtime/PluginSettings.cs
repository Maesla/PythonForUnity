namespace MicrokernelSystem
{
    /// <summary>
    /// This struct provides the necessary data for a IMicrokernel Execution
    /// </summary>
    /// <see cref="IMicrokernel.Execute(PluginSettings)"/>
    /// <seealso cref="IMicrokernel.Create(PluginSettings)"/>
    public struct PluginSettings 
    {
        /// <summary>
        /// The plugin type
        /// </summary>
        public string type;
        
        /// <summary>
        /// The plugin version
        /// </summary>
        public SemanticVersioning version;
        
        /// <summary>
        /// The code to be executed
        /// </summary>
        public string code;
        
        /// <summary>
        /// The plugin id
        /// </summary>
        public string id;

        public static PluginSettings Default()
        {
            return new PluginSettings { type = "Python", version = new SemanticVersioning("0.0.0") , code = string.Empty};
        }
    } 
}
