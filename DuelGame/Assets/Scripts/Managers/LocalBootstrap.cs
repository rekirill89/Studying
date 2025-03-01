using System;
using UnityEngine;

namespace DuelGame
{
    public class LocalBootstrap : MonoBehaviour
    {
        [SerializeField] private LocalUIEffectsFacade uiFacade;
        [SerializeField] private LocalBattleManagerFacade battleFacade;
        [SerializeField] private LocalUIManagerFacade stateUIFacade;

        private LocalBattleManager _battleManager;
        private LocalUIEffectsManager _uIManager;
        private LocalUIManager _gameStateUIManager;

        // Get from service locator
        private CorutineManager _corutineManager;
        private GameEventManager _gameEventManager;
        private EntityFactory _entityFactory;

        public event Action OnReady;

        private void Awake()
        {
            _gameEventManager = ServiceLocator.Get<GameEventManager>();
            _entityFactory = ServiceLocator.Get<EntityFactory>();
            _corutineManager = ServiceLocator.Get<CorutineManager>();

            _battleManager = new LocalBattleManager(_gameEventManager, _corutineManager, _entityFactory);
            _uIManager = new LocalUIEffectsManager(_gameEventManager, _corutineManager);
            _gameStateUIManager = new LocalUIManager(_gameEventManager);
        }

        private void Start()
        {            
            _battleManager.InitializeBattleElements(battleFacade);
            _uIManager.InitializeUIElementsNObjectPool(uiFacade);
            _gameStateUIManager.InitializeStateUIElements(stateUIFacade);

            OnReady += _gameStateUIManager.OnReadyMethod();

            OnReady?.Invoke();
        }
    }
}

