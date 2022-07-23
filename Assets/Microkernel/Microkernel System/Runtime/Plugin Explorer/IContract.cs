namespace MicrokernelSystem
{
    /// <summary>
    /// Relevant plugin information
    /// </summary>
    public interface IContract : IValidate
    {
        string Name { get; }
        string Id { get; }
        string Author { get; }
        SemanticVersioning Version { get; }
    } 
}
