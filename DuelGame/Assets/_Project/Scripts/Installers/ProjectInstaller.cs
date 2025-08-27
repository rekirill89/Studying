using Firebase.Analytics;
using Unity.Services.CloudSave;
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
        [SerializeField] private AssetReference _skinsRef;
        
        [SerializeField] private AssetReference _panelsRef;
        [SerializeField] private AssetReference _hudCanvasRef;
        [SerializeField] private AssetReference _screenCanvasRef;

        
        public override void InstallBindings()
        {
            Debug.Log("Project installer created");

            Container.BindInterfacesAndSelfTo<PurchasesDataController>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<RemoteConfigsLoader>().AsSingle().WithArguments(_battleConfigRef);
            Container.BindInterfacesAndSelfTo<FireBaseInit>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkinsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkinAssetsLoader>().AsSingle().WithArguments(
                _skinsRef);
            Container.BindInterfacesAndSelfTo<GlobalAssetsLoader>().AsSingle().WithArguments(
                _buffsRef, _heroesRef, _panelsRef, _hudCanvasRef, _screenCanvasRef);            
            Container.BindInterfacesAndSelfTo<LocalAssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AuthService>().AsSingle();
            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<UnityCloudSaveService>().AsSingle();
            Container.Bind<DataCache>().AsSingle(); 
            Container.Bind<InternetConnector>().AsSingle();
        }
    }
}