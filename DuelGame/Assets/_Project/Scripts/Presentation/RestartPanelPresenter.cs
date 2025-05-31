using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class RestartPanelPresenter :IDisposable
    {
        private readonly BattleManager _battleManagerModel;
        private readonly RestartPanelView _restartPanelView;
        private readonly SceneLoaderManager _sceneLoaderManager;

        public RestartPanelPresenter(BattleManager battleManager, SceneLoaderManager sceneLoaderManager, RestartPanelView restartPanelView)
        {
            _battleManagerModel = battleManager;
            _restartPanelView = restartPanelView;
            _sceneLoaderManager = sceneLoaderManager;
            
            _restartPanelView.OnButtonClicked += _sceneLoaderManager.LoadBattleScene;
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
