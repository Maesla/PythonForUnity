using System;

namespace MicrokernelSystem
{
    /// <summary>
    /// A IMicrokernel Execution
    /// </summary>
    /// <see cref="IMicrokernel.Execute(PluginSettings)"/>
    public class Run
    {
        /// <summary>
        /// The runnableContext the ran the plugin
        /// </summary>
        public IRunnableContext runnableContext;
        
        /// <summary>
        /// The plugin that were executed
        /// </summary>
        public PluginSettings settings;
        
        /// <summary>
        /// The moment that the plugin was ran
        /// </summary>
        public DateTime timestamp;
        
        /// <summary>
        /// The execution result
        /// </summary>
        public string result;

        /// <summary>
        /// The objects that were written in the buffer in this execution
        /// </summary>
        public object [] objs;

        public Run() : this(null, string.Empty, new PluginSettings(), Array.Empty<object>()){}

        public Run(IRunnableContext runnableContext, string result, PluginSettings settings, object [] objs)
        {
            this.runnableContext = runnableContext;
            this.settings = settings;
            timestamp = DateTime.Now;
            this.result = result;
            this.objs = objs;
        }


        public static implicit operator string(Run r) => r.result;

        public override string ToString()
        {
            string headline = $"Run at {timestamp}. Running {settings.type}, plugin: {settings.id}. Created: {objs.Length} objects";
            return $"{headline}";
        }
    } 
}
