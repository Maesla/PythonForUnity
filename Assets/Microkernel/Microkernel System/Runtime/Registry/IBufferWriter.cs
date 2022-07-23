namespace MicrokernelSystem
{
    /// <summary>
    /// This interface gives a mechanism to the providers to write something that could be used and referenced in the following executions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBufferWriter<T>
    {
        void Write(T t);
    } 
}
