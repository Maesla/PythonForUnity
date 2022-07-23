namespace MicrokernelSystem
{
    public static class PluginUtils
    {
        public static PluginSettings GeneratePluginSettings(this IPlugin plugin)
        {
            PluginSettings settings = new PluginSettings()
            {
                type = "Python",
                code = plugin.Code.GetCode(),
                version = new SemanticVersioning("0.0.0"),
                id = plugin.Contract.Id
            };

            return settings;
        }
    } 
}
