using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class RestartPanelPresenter : IPresenter<RestartView>
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonView _restartView;
        private readonly SceneLoaderService _sceneLoaderService;

        public RestartPanelPresenter(
            BattleManager battleManager, 
            SceneLoaderService sceneLoaderService, 
            RestartView restartView)
        {
            _battleManagerModel = battleManager;
            _restartView = restartView;
            _sceneLoaderService = sceneLoaderService;
        }
                                       
        public void Initialize()
        {
            _restartView.OnButtonClicked += _sceneLoaderService.LoadBattleScene;
            _battleManagerModel.OnBattleFinish += ShowView;
            _battleManagerModel.BattleStateModel.OnStateChanged += StateChangedHandler;
        }

        public void Dispose()
        {
            _restartView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
            _battleManagerModel.BattleStateModel.OnStateChanged -= StateChangedHandler;
        }

        public void ShowView(Players? playerWhoLost)
        {
            if (playerWhoLost == Players.Player1)
            {
                _restartView.Show();
            }
        }
        
        private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _restartView.Hide();
        }
    }   
}