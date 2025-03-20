using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSceneInstaller : MonoInstaller
    {        
        [SerializeField] private BattleManagerFacade battleFacade;
        [SerializeField] private BattleSceneUIManagerFacade uiFacade;
        
        public override void InstallBindings()
        {
            Debug.Log("Battle scene installer created");
            Container.BindInterfacesAndSelfTo<BattleManager>().AsSingle().WithArguments(battleFacade);
            Container.BindInterfacesAndSelfTo<BattleSceneUIManager>().AsSingle().WithArguments(uiFacade);
            Container.BindInterfacesAndSelfTo<BuffsManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameStateController>().AsCached().NonLazy();
        }
        
        private void OnDestroy()
        {
            Debug.Log("Battle scene installer destroyed");
        }
    }
}

