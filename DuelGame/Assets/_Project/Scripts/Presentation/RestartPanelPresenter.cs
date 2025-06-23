using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class RestartPanelPresenter : IDisposable
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
        }
        
        public void Dispose()
        {
            _restartPanelView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
        }

        public void ShowView(Players playerWhoLost)
        {
            if (playerWhoLost == Players.Player1)
            {
                _restartPanelView.Show();
            }
        }
    }   
}