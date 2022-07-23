using System;

namespace MicrokernelSystem
{
    internal class Plugin : IPlugin
    {
        public IContract Contract { get; set; }

        public ICode Code { get; set; }

        public ICode UI { get; set; }

        public IICon Icon { get; set; }

        public bool Validate()
        {
            if (Contract == null)
            {
                throw new ArgumentException("Contract is null");
            }
            if (Code == null)
            {
                throw new ArgumentException("Code is null");
            }

            return Contract.Validate();
        }
    } 
}
