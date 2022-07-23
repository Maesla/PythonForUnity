using System;

namespace MicrokernelSystem
{
    public interface IRunnableContext : IDisposable
    {
        /// <summary>
        /// Execute a scriptable code
        /// </summary>
        /// <returns>the execution result</returns>
        string Execute();
    } 
}
