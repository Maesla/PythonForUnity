using System;
using System.Collections;
using System.Collections.Generic;

namespace MicrokernelSystem
{
    public class Registry : IRegistry
    {
        public event Action<Run> OnRunAdded;
        private readonly List<Run> list;

        public Registry()
        {
            list = new List<Run>();
        }


        public int Count => list.Count;

        public void Add(Run run)
        {
            list.Add(run);
            OnRunAdded?.Invoke(run);
        }

        public IEnumerator<Run> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    } 
}
