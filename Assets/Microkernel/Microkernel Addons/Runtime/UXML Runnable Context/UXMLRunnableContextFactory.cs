namespace MicrokernelSystem.Addons
{
    internal class UXMLRunnableContextFactory : BaseRunnableContextFactory
    {
        public override string Type => "UXML";

        public override IRunnableContext Create(PluginSettings settings)
        {
            return new UXMLRunnableContext(settings.code);
        }
    }

}