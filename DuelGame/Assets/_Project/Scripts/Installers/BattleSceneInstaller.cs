using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleManagerFacade _battleFacade;

        [SerializeField] private StartPanelFacade _startPanelFacade;
        [SerializeField] private RestartPanelFacade _restartPanelFacade;
        [SerializeField] private ContinuePanelFacade _continuePanelFacade;
        [SerializeField] private ReloadButtonFacade _reloadButtonFacade;
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleSceneUIManager>().AsSingle().WithArguments(
                _startPanelFacade,
                _restartPanelFacade,
                _continuePanelFacade,
                _reloadButtonFacade);

            Container.BindInterfacesAndSelfTo<BattleScenePresenter>().AsCached();
        }
        
        private void OnDestroy()
        {
            Debug.Log("Battle scene installer destroyed");
        }
    }
}

