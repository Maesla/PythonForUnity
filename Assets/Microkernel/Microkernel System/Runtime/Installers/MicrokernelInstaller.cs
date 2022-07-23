using Zenject;

namespace MicrokernelSystem
{
    public class MicrokernelInstaller : Installer<MicrokernelInstaller>
    {
        public override void InstallBindings()
        {
            Buffer buffer = new Buffer();
            Container.Bind<IBufferWriter>().FromInstance(buffer);
            Container.Bind<Buffer>().FromInstance(buffer).WhenInjectedInto<Microkernel>();

            Container.Bind<IRegistry>().To<Registry>().AsSingle();

            Container
                .Bind<IMicrokernel>()
                .To<Microkernel>()
                .AsSingle();
        }
    }
}
