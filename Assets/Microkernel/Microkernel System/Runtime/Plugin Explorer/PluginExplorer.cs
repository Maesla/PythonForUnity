using System;
using System.Collections.Generic;
using System.IO;

namespace MicrokernelSystem
{
    internal class PluginExplorer : IPluginExplorer
    {
        private const string contractFileName = @"contract.json";
        private const string codeFileName = @"code.py";
        private const string uiFileName = @"ui.json";
        private const string iconFileName = @"icon.png";

        public IPlugin[] Plugins { get; private set; }

        public string [] RootPaths { get; set; }

        public void Explore()
        {

            List<IPlugin> plugins = new List<IPlugin>();

            foreach (string path in RootPaths)
            {
                Explore(plugins, path);
            }

            CheckUniqueIds(plugins);

            Plugins = plugins.ToArray();
        }

        internal void CheckUniqueIds(IEnumerable<IPlugin> plugins)
        {
            HashSet<string> hashset = new HashSet<string>();
            foreach (IPlugin p in plugins)
            {
                if (!hashset.Add(p.Contract.Id))
                {
                    string msg = $"Duplicated Id: {p.Contract.Id} found in Plugin: {p.Contract.Name}. Please check";
                    throw new InvalidOperationException(msg);
                }
            }
        }

        private void Explore(List<IPlugin> plugins, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                foreach (DirectoryInfo info in directoryInfo.GetDirectories())
                {
                    IPlugin p = GetPlugin(info.FullName);

                    if (p.Validate())
                    {
                        plugins.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a plugin from the directory
        /// </summary>
        private IPlugin GetPlugin(string directory)
        {
            string contractPath = Path.Combine(directory, contractFileName);
            Contract contract = Contract.Create(contractPath);

            string codePath = Path.Combine(directory, codeFileName);
            Code code = new Code(codePath);

            string uiPath = Path.Combine(directory, uiFileName);
            Code ui = new Code(uiPath);

            string iconPath = Path.Combine(directory, iconFileName);
            ICon icon = new ICon(iconPath);

            Plugin plugin = new Plugin()
            {
                Contract = contract,
                Code = code,
                UI = ui,
                Icon = icon
            };

            return plugin;
        }
    } 
}