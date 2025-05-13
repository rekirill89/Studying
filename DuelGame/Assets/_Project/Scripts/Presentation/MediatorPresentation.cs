using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class MediatorPresentation : IDisposable
    {
        private readonly DiContainer _container;
        private readonly UIFactory _uiFactory;
        private readonly BattleManager _battleManagerModel;
        
        private StartPanelPresenter _startPanelPresenter;
        private ContinuePanelPresenter _continuePanelPresenter;
        private RestartPanelPresenter _restartPanelPresenter;
        private ReloadButtonPresenter _reloadButtonPresenter;

        public MediatorPresentation(DiContainer container, UIFactory uiFactory, BattleManager battleManagerModel)
        {
            _container = container;
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
            TryCreateReloadButton();
        }

        private void BattleFinishHandler(Players playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                TryCreateContinuePanel();
            }
            else
            {
                TryCreateRestartPanel();
            }
        }
        
        private void TryCreateStartPanel()
        {
            _startPanelPresenter ??= _container.Instantiate<StartPanelPresenter>(
                new object[] { _uiFactory.CreateStartPanelView() }
            );
        }
        
        private void TryCreateRestartPanel()
        {
            _restartPanelPresenter ??= _container.Instantiate<RestartPanelPresenter>(
                new object[] { _uiFactory.CreateRestartPanelView() }
            );
        }
        
        private void TryCreateContinuePanel()
        {
            _continuePanelPresenter ??= _container.Instantiate<ContinuePanelPresenter>(
                new object[] { _uiFactory.CreateContinuePanelView() }
            );
        }
        
        private void TryCreateReloadButton()
        {
            _reloadButtonPresenter ??= _container.Instantiate<ReloadButtonPresenter>(
                new object[] { _uiFactory.CreateReloadButtonView() }
            );
        }
    }   
}
