using PythonEngineSystem;
using Zenject;

namespace MicrokernelSystem.Addons
{
    internal class PythonInstaller : Installer<MicrokernelSystemSettings, PythonInstaller>
    {
        private readonly MicrokernelSystemSettings microkernelSettings;
        private const string pythonEngineSettingsKey = "pythonEngineSettings";

        public PythonInstaller(MicrokernelSystemSettings settings)
        {
            this.microkernelSettings = settings;
        }

        public override void InstallBindings()
        {
            InstallPythonSetup();
            InstallLogger();
        }

        private void InstallPythonSetup()
        {
            IPythonEngineSettings pythonSettings = microkernelSettings.GetSubSettings(pythonEngineSettingsKey).FromJson<PythonEngineSettings>();
            PythonSetup setup = new PythonSetup(pythonSettings);
            Container.Bind<IPythonSetup>().To<PythonSetup>().FromInstance(setup);
        }

        private void InstallLogger()
        {
            Container.Bind<IPythonLogger>().To<PythonLogger>().AsSingle();
        }
    } 
}
