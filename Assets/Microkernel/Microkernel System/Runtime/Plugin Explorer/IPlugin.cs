namespace MicrokernelSystem
{
    /// <summary>
    /// Each of the system plugins
    /// </summary>
    public interface IPlugin : IValidate 
    {
        IContract Contract { get; }
        ICode Code { get; }
        ICode UI { get; }

        IICon Icon { get; }
    } 
}
