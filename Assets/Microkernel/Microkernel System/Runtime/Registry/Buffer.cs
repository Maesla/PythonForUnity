using System;
using System.Collections.Generic;

namespace MicrokernelSystem
{
    public interface IBufferWriter : IBufferWriter<object>{ }

    public class Buffer : Buffer<object>, IBufferWriter {}

    /// <summary>
    /// Main implementation of the buffer. The buffer is open when a execution starts and it is closed when the execution finished
    /// </summary>
    /// <see cref="Microkernel.Execute(PluginSettings)"/>
    public class Buffer<T> : IBufferWriter<T>
    {
        private readonly List<T> list;
        public bool IsOpen { get; private set; }
        public int Count => list.Count;

        public Buffer()
        {
            list = new List<T>();
            IsOpen = false;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Write(T t)
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("The buffer is not open");
            }

            list.Add(t);
        }

        public T[] Flush()
        {
            var data = list.ToArray();
            list.Clear();
            return data;
        }

        public T[] Close()
        {
            IsOpen = false;
            return Flush();
        }
    } 
}
