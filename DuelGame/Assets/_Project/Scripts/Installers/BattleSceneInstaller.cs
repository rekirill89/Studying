using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleSettingsFacade _battleFacade;
        [SerializeField] private StartPanelFacade _startPanelFacade;
        [SerializeField] private RestartPanelFacade _restartPanelFacade;
        [SerializeField] private ContinuePanelFacade _continuePanelFacade;
        [SerializeField] private ReloadButtonFacade _reloadButtonFacade;
                
        private void OnDestroy()
        {
            Debug.Log("Battle scene installer destroyed");
        }
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroesService>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleSceneUIManager>().AsSingle().WithArguments(
                _startPanelFacade,
                _restartPanelFacade,
                _continuePanelFacade,
                _reloadButtonFacade);

            Container.BindInterfacesAndSelfTo<BattleScenePresenter>().AsCached();
        }

    }
}

