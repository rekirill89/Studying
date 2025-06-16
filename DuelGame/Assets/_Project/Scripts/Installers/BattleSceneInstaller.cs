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
        
        [SerializeField] private StartPanelView _startPanelView;
        [SerializeField] private ContinuePanelView _continuePanelView;
        [SerializeField] private RestartPanelView _restartPanelView;
        [SerializeField] private ReloadPanelView _reloadPanelView;
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<SceneLoaderManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle().WithArguments(
                _screenCanvasParent,
                _hudCanvasParent,
                _startPanelView,
                _continuePanelView,
                _restartPanelView,
                _reloadPanelView);
            
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroesService>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle().WithArguments(_battleFacade);

            Container.BindInterfacesAndSelfTo<MediatorPresentation>().AsSingle();
        }
    }
}