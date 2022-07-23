using Zenject;

namespace MicrokernelSystem.Addons
{
    public class MicrokernelAddonsMonoInstaller : MonoInstaller<MicrokernelAddonsMonoInstaller>
    {
        [Inject]
        private MicrokernelSystemSettings settings;
        public override void InstallBindings()
        {
            PythonInstaller.Install(Container, settings);
        }
    } 
}
