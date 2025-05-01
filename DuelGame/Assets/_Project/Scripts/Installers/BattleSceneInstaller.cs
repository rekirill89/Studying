using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleSettingsFacade _battleFacade;
        
        [SerializeField] private HUDCanvasFacade _hudCanvasFacade;
        [SerializeField] private ScreenCanvasFacade _screenCanvasFacade;
        [SerializeField] private Transform _canvasParent;
        private void OnDestroy()
        {
            Debug.Log("Battle scene installer destroyed");
        }
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle().WithArguments(_hudCanvasFacade, _screenCanvasFacade, _canvasParent);
            Container.BindInterfacesAndSelfTo<BattleStateModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroesService>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle().WithArguments(_battleFacade);
            Container.BindInterfacesAndSelfTo<BattleSceneUIManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleScenePresenter>().AsCached();
        }

    }
}

