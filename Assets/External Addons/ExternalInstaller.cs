using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MicrokernelSystem.Addons.External
{
    public class ExternalInstaller : MonoInstaller<ExternalInstaller>
    {
        [SerializeField]
        private Image sourceImage;

        public override void InstallBindings()
        {
            Container.Bind<Image>().WithId("sourceImage").FromInstance(sourceImage);
        }
    }
}
