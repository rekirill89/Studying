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
        [SerializeField] private AssetReference _battleConfigRef;
        [SerializeField] private AssetReference _buffsRef;
        [SerializeField] private AssetReference _heroesRef; 
        
        public override void InstallBindings()
        {
            Debug.Log("Project installer created");

            Container.BindInterfacesAndSelfTo<GlobalBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<RemoteConfigsManager>().AsSingle().WithArguments(_battleConfigRef);
            Container.BindInterfacesAndSelfTo<FireBaseInit>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalAssetsLoader>().AsSingle().WithArguments(_buffsRef, _heroesRef);            
            Container.BindInterfacesAndSelfTo<LocalAssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<BattleDataCache>().AsSingle(); 
            

        }
    }
}