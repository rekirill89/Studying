using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleSettingsFacade _battleFacade;
        
        [FormerlySerializedAs("_panels")] [SerializeField] private AssetReference _panelsRef;
        [SerializeField] private AssetReference _bloodPrefabRef;
        [SerializeField] private AssetReference _deathEffectRef;
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            
            Container.BindInterfacesAndSelfTo<BattleSceneEntryPoint>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleSessionContext>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<AnalyticsDataCollector>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSceneAssetsLoader>().AsSingle().WithArguments(
                _battleFacade, 
                _bloodPrefabRef,
                _deathEffectRef);
            Container.BindInterfacesAndSelfTo<HeroesLifecycleController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleDataController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleVFXController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<BattleSFXController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MediatorPresentation>().AsSingle();
        }
    }
}