using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleSettingsFacade _battleFacade;
        
        [SerializeField] private Transform _screenCanvasParent;
        [SerializeField] private Transform _hudCanvasParent;
        
        [SerializeField] private Panels _panels;
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle().WithArguments(
                _screenCanvasParent,
                _hudCanvasParent,
                _panels);
            
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();            
            Container.BindInterfacesAndSelfTo<BattleSessionContext>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<HeroesLifecycleController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleDataController>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticsDataCollector>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MediatorPresentation>().AsSingle();
        }
    }
}