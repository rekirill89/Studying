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
        [SerializeField] private AssetReferenceContainer _assetReferenceContainer;
        
        public override void InstallBindings()
        {
            Debug.Log("Project installer created");

            Container.BindInterfacesAndSelfTo<PurchasesDataController>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<RemoteConfigsLoader>().AsSingle().WithArguments(_assetReferenceContainer.BattleConfigRef);
            Container.BindInterfacesAndSelfTo<FireBaseInit>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkinsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BackgroundMusicController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SkinAssetsLoader>().AsSingle().WithArguments(_assetReferenceContainer.SkinsRef);
            Container.BindInterfacesAndSelfTo<AudioAssetsLoader>().AsSingle().WithArguments(
                _assetReferenceContainer.SoundEffectsRef,
                _assetReferenceContainer.MusicsRef,
                _assetReferenceContainer.MusicPlayerRef);
            Container.BindInterfacesAndSelfTo<GlobalAssetsLoader>().AsSingle().WithArguments(_assetReferenceContainer);            
            
            Container.BindInterfacesAndSelfTo<LocalAssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AuthService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InternetConnector>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneState>().AsSingle();
            
            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<UnityCloudSaveService>().AsSingle();
            Container.Bind<DataCache>().AsSingle(); 
        }
    }

    [System.Serializable]
    public class AssetReferenceContainer
    {
        public AssetReference BattleConfigRef;
        
        public AssetReference BuffsRef;
        public AssetReference HeroesRef; 
        
        public AssetReference SkinsRef;
        public AssetReference PanelsRef;
        public AssetReference SoundEffectsRef;
        public AssetReference MusicsRef;
        public AssetReference MusicPlayerRef;
        
        public AssetReference HUDCanvasRef;
        public AssetReference ScreenCanvasRef;
    }
}