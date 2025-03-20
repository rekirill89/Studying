using UnityEngine;
using Zenject;

namespace  DuelGame
{
    public class LoadingSceneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInitializable>().To<NextSceneLoader>().AsCached().NonLazy();
        }
    }   
}
