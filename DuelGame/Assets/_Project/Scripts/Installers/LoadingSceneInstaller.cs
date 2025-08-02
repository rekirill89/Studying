using UnityEngine;
using Zenject;

namespace  DuelGame
{
    public class LoadingSceneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MenuSceneEntryPoint>().AsCached();
        }
    }   
}