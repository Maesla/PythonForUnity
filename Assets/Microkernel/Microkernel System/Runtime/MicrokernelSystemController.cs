using System;
using Zenject;

namespace MicrokernelSystem
{
    /// <summary>
    /// This class perform as an orchestrator. It inits all the the system and finishes it too.
    /// </summary>
    public class MicrokernelSystemController : IInitializable, IDisposable
    {
        private readonly IMicrokernel microkernel;

        public MicrokernelSystemController(IMicrokernel microkernel)
        {
            this.microkernel = microkernel;
        }

        public void Initialize()
        {
            microkernel.Initialize();
        }

        public void Dispose()
        {
            microkernel.Finish();
        }
    }
}