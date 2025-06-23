using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class MediatorPresentation : IDisposable
    {
        private readonly IInstantiator _iInstantiator;
        private readonly UIFactory _uiFactory;
        private readonly BattleManager _battleManagerModel;
        
        private StartPanelPresenter _startPanelPresenter;
        private ContinuePanelPresenter _continuePanelPresenter;
        private RestartPanelPresenter _restartPanelPresenter;
        private ReloadPanelPresenter _reloadPanelPresenter;
        private SavePanelPresenter _savePanelPresenter;
        private LoadPanelPresenter _loadPanelPresenter;

        public MediatorPresentation(IInstantiator iInstantiator, UIFactory uiFactory, BattleManager battleManagerModel)
        {
            _iInstantiator = iInstantiator;
            _uiFactory = uiFactory;
            _battleManagerModel = battleManagerModel;

            _battleManagerModel.OnBattleReady += BattleReadyHandler;
            _battleManagerModel.OnPlayersSpawned += PlayerSpawnedHandler;
            _battleManagerModel.OnBattleFinish += BattleFinishHandler;
        }
        
        public void Dispose()
        {
            _battleManagerModel.OnBattleReady -= BattleReadyHandler;
            _battleManagerModel.OnPlayersSpawned -= PlayerSpawnedHandler;
            _battleManagerModel.OnBattleFinish -= BattleFinishHandler;
        }

        private void BattleReadyHandler()
        {
            TryCreatePanel(
                ref _startPanelPresenter,
                () => _uiFactory.CreateStartPanelView(),
                presenter => presenter.ShowView());
            TryCreatePanel(
                ref _reloadPanelPresenter,
                () => _uiFactory.CreateReloadPanelView(),
                presenter => presenter.ShowView());
        }

        private void PlayerSpawnedHandler(BattleState _)
        {
            TryCreatePanel(
                ref _savePanelPresenter,
                () => _uiFactory.CreateSavePanelView(),
                presenter => presenter.ShowView());
        }

        private void BattleFinishHandler(Players playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                TryCreatePanel(
                    ref _continuePanelPresenter, 
                    () => _uiFactory.CreateContinuePanelView(),
                    presenter => presenter.ShowView(playerWhoLost));
            }
            else
            {
                TryCreatePanel(
                    ref _restartPanelPresenter, 
                    () => _uiFactory.CreateRestartPanelView(),
                    presenter => presenter.ShowView(playerWhoLost));
            }
            
            TryCreatePanel(
                ref _loadPanelPresenter,
                () => _uiFactory.CreateLoadPanelView(),
                presenter => presenter.ShowView(playerWhoLost));
        }
        
        private void TryCreatePanel<TPresenter, TView>(
            ref TPresenter presenter,
            Func<TView> createView,
            Action<TPresenter> showView)
            where TPresenter : class
            where TView : class
        {
            if(presenter != null)
                return;
            
            presenter = _iInstantiator.Instantiate<TPresenter>(
                new object[] { createView() });
            showView(presenter);
        }
    }   
}