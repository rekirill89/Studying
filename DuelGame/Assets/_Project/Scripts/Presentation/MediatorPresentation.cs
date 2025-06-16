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

        public MediatorPresentation(IInstantiator iInstantiator, UIFactory uiFactory, BattleManager battleManagerModel)
        {
            _iInstantiator = iInstantiator;
            _uiFactory = uiFactory;
            _battleManagerModel = battleManagerModel;

            _battleManagerModel.OnBattleReady += BattleReadyHandler;
            _battleManagerModel.OnBattleFinish += BattleFinishHandler;
        }
        
        public void Dispose()
        {
            _battleManagerModel.OnBattleReady -= BattleReadyHandler;
            _battleManagerModel.OnBattleFinish -= BattleFinishHandler;
        }

        private void BattleReadyHandler()
        {
            TryCreateStartPanel();
            TryCreateReloadPanel();
        }

        private void BattleFinishHandler(Players playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                TryCreateContinuePanel(playerWhoLost);
            }
            else
            {
                TryCreateRestartPanel(playerWhoLost);
            }
        }
        
        private void TryCreateStartPanel()
        {
            if (_startPanelPresenter != null) 
                return;
            
            _startPanelPresenter = _iInstantiator.Instantiate<StartPanelPresenter>(
                new object[] { _uiFactory.CreateStartPanelView() });
            _startPanelPresenter.ShowView();
        }
        
        private void TryCreateRestartPanel(Players playerWhoLost)
        {
            if (_restartPanelPresenter != null)
                return;
            
            _restartPanelPresenter = _iInstantiator.Instantiate<RestartPanelPresenter>(
                new object[] { _uiFactory.CreateRestartPanelView() });
            _restartPanelPresenter.ShowView(playerWhoLost);
        }
        
        private void TryCreateContinuePanel(Players playerWhoLost)
        {
            if (_continuePanelPresenter != null)
                return;
            
            _continuePanelPresenter = _iInstantiator.Instantiate<ContinuePanelPresenter>(
                new object[] { _uiFactory.CreateContinuePanelView() });
            _continuePanelPresenter.ShowView(playerWhoLost);
        }
        
        private void TryCreateReloadPanel()
        {
            if (_reloadPanelPresenter != null)
                return;
            
            _reloadPanelPresenter = _iInstantiator.Instantiate<ReloadPanelPresenter>(
                new object[] { _uiFactory.CreateReloadButtonView() });
            _reloadPanelPresenter.ShowView();
        }
    }   
}