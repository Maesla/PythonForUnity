using System;
using UnityEngine;
using Zenject;

namespace MicrokernelSystem.UI
{
    public class MicrokernelUIInstaller : MonoInstaller<MicrokernelUIInstaller>
    {
        [SerializeField]
        private GameObject microkernelCanvasPrefab;

        public override void InstallBindings()
        {
            if (microkernelCanvasPrefab != null)
            {
                GetCanvasBindings()
                    .FromComponentInNewPrefab(microkernelCanvasPrefab)
                    .AsSingle()
                    .NonLazy();
            }
            else
            {
                var pluginWindowController = FindObjectOfType<PluginWindowController>();
                GetCanvasBindings().FromInstance(pluginWindowController);

            }
        }

        private ConcreteIdBinderNonGeneric GetCanvasBindings()
        {
            return Container.Bind(typeof(IPluginWindowController), typeof(Console));
        }


    }

}