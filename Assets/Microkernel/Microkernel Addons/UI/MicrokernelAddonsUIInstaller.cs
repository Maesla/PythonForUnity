using PythonEngineSystem;
using Zenject;

namespace MicrokernelSystem.Addons.UI
{
    public class MicrokernelAddonsUIInstaller : MonoInstaller<MicrokernelAddonsUIInstaller>
    {

        public override void InstallBindings()
        {
            Container.Decorate<IPythonLogger>().With<PythonConsole>();
        }
    }
}
