using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class RestartPanelPresenter : IDisposable, IPresenter
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonPanelView _restartPanelView;
        private readonly SceneLoaderService _sceneLoaderService;

        public RestartPanelPresenter(
            BattleManager battleManager, 
            SceneLoaderService sceneLoaderService, 
            RestartPanelView restartPanelView)
        {
            _battleManagerModel = battleManager;
            _restartPanelView = restartPanelView;
            _sceneLoaderService = sceneLoaderService;
            
            _restartPanelView.OnButtonClicked += _sceneLoaderService.LoadBattleScene;
            _battleManagerModel.OnBattleFinish += ShowView;
            _battleManagerModel.BattleStateModel.OnStateChanged += StateChangedHandler;
        }
        
        public void Dispose()
        {
            _restartPanelView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
            _battleManagerModel.BattleStateModel.OnStateChanged -= StateChangedHandler;
        }

        public void ShowView(Players? playerWhoLost)
        {
            if (playerWhoLost == Players.Player1)
            {
                _restartPanelView.Show();
            }
        }
        
        private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _restartPanelView.Hide();
        }
    }   
}