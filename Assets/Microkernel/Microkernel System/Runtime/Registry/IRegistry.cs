using System;
using System.Collections.Generic;

namespace MicrokernelSystem
{
    /// <summary>
    /// A set of runs
    /// </summary>
    public interface IRegistry : IEnumerable<Run>
    {
        event Action<Run> OnRunAdded;
        void Add(Run run);

        int Count { get; }
    }
}
