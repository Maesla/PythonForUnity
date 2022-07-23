namespace MicrokernelSystem
{
    /// <summary>
    /// Plugin code
    /// </summary>
    public interface ICode
    {
        string Path { get; }
        void SetCode(string code);
        string GetCode();

        void OpenInDefaultApplication();
        void Reload();
    } 
}
