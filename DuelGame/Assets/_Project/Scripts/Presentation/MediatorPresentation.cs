using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class MediatorPresentation : IDisposable
    {
        private readonly IInstantiator _iInstantiator;
        private readonly UIFactory _uiFactory;
        private readonly BattleManager _battleManagerModel;
        private readonly Dictionary<Type, Action<Players?>> _presentersActions;
        private readonly List<IPresenter> _presenters = new List<IPresenter>();

        private StartPanelPresenter _startPanelPresenter;
        private ContinuePanelPresenter _continuePanelPresenter;
        private RestartPanelPresenter _restartPanelPresenter;
        private ReloadPanelPresenter _reloadPanelPresenter;
        private SavePanelPresenter _savePanelPresenter;
        private LoadPanelPresenter _loadPanelPresenter;
        private AdsPanelPresenter _adsPanelPresenter;
        
        public MediatorPresentation(IInstantiator iInstantiator, UIFactory uiFactory, BattleManager battleManagerModel)
        {
            _iInstantiator = iInstantiator;
            _uiFactory = uiFactory;
            _battleManagerModel = battleManagerModel;
            
            _presentersActions = new Dictionary<Type, Action<Players?>>()
            {
                {typeof(StartPanelPresenter), _ => _startPanelPresenter?.ShowView()},
                {typeof(ReloadPanelPresenter), _ => _reloadPanelPresenter?.ShowView()},                
                {typeof(SavePanelPresenter), _ => _savePanelPresenter?.ShowView()},
                {typeof(RestartPanelPresenter), playerWhoLost => _restartPanelPresenter?.ShowView(playerWhoLost)},
                {typeof(ContinuePanelPresenter), playerWhoLost => _continuePanelPresenter?.ShowView(playerWhoLost)},
                {typeof(LoadPanelPresenter), playerWhoLost => _loadPanelPresenter?.ShowView(playerWhoLost)},
                {typeof(AdsPanelPresenter), playerWhoLost => _adsPanelPresenter?.ShowView(playerWhoLost)}
            };

            _battleManagerModel.OnBattleReady += BattleReadyHandler;
            _battleManagerModel.OnPlayersSpawned += PlayerSpawnedHandler;
            _battleManagerModel.OnBattleFinish += BattleFinishHandler;
        }
        
        public void Dispose()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Dispose();
            }
            
            _battleManagerModel.OnBattleReady -= BattleReadyHandler;
            _battleManagerModel.OnPlayersSpawned -= PlayerSpawnedHandler;
            _battleManagerModel.OnBattleFinish -= BattleFinishHandler;
            Debug.Log("Presenter cleaned");
        }

        private void BattleReadyHandler()
        {
            TryCreatePanel(ref _startPanelPresenter, null);
            
            TryCreatePanel(ref _reloadPanelPresenter, null);
        }

        private void PlayerSpawnedHandler(BattleState _)
        {
            TryCreatePanel(ref _savePanelPresenter, null);
        }

        private void BattleFinishHandler(Players? playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                TryCreatePanel(ref _continuePanelPresenter, playerWhoLost);
            }
            else
            {
                TryCreatePanel(ref _restartPanelPresenter, playerWhoLost);
            }
            
            TryCreatePanel(ref _loadPanelPresenter, playerWhoLost);
            
            TryCreatePanel(ref _adsPanelPresenter, playerWhoLost);
        }
        
        private bool TryCreatePanel<TPresenter>(
            ref TPresenter presenter,
            Players? playerWhoLost) 
            where TPresenter : IPresenter
        {
            if(presenter != null)
                return false;
            presenter = _iInstantiator.Instantiate<TPresenter>(
                new object[] { _uiFactory.CreatePanelView<TPresenter>() });
            _presenters.Add(presenter);
            
            _presentersActions[typeof(TPresenter)].Invoke(playerWhoLost);
            return true;
        }
    }   
}