using System;

namespace MicrokernelSystem.Addons.External
{
    public class EventProvider : BasePythonAPIProvider
    {
        public void Load(Action<string> callback)
        {
            callback("callback called from EventProvider");
        }
    } 
}
