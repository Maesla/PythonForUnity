namespace MicrokernelSystem
{
    /// <summary>
    /// An interface to be used by the differents IRunnableContexts
    /// </summary>
    /// <remarks>
    /// The RunnableContextFactories will receive IAPIProviders and they will pass them to the contexts.
    /// The contexts will use them according to their own implementation
    /// </remarks>
    public interface IAPIProvider
    {
        string[] Types { get; }
        SemanticVersioning Version { get; }
    } 
}
