//using UnityEditor.Overlays;

using Firebase.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _buffsRef;
        [SerializeField] private AssetReference _heroesRef; 
        
        public override void InstallBindings()
        {
            Debug.Log("Project installer created");
            
            Container.Bind<ILocalAssetLoader>().To<LocalAssetLoader>().AsSingle();
            //Container.Bind<AdConfig>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalAssetsLoader>().AsSingle().WithArguments(_buffsRef, _heroesRef);            
            Container.Bind<IAdService>().To<AdService>().AsSingle();
            Container.Bind<IAnalyticService>().To<AnalyticService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<BattleDataCache>().AsSingle(); 
            

        }
    }
}