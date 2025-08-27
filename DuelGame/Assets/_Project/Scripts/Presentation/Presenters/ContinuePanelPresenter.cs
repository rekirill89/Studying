using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class ContinuePanelPresenter : IPresenter<ContinueView>
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonView _continueView;

        public ContinuePanelPresenter(BattleManager battleManager, ContinueView continueView)
        {
            _battleManagerModel = battleManager;
            _continueView = continueView;
        }
        
        public void Initialize()
        {
            _continueView.OnButtonClicked += _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish += ShowView;
            _battleManagerModel.OnPlayersSpawned += HideView;
            _battleManagerModel.BattleStateModel.OnStateChanged += StateChangedHandler;      
        }
        
        public void Dispose()
        {
            _continueView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
            _battleManagerModel.OnPlayersSpawned -= HideView;
            _battleManagerModel.BattleStateModel.OnStateChanged -= StateChangedHandler; 
        }

        public void ShowView(Players? playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                _continueView.Show();
            }
        }
        
        private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _continueView.Hide();
        }
        
        private void HideView(BattleState battleState)
        {
            if (battleState == BattleState.Continued)
            {
                _continueView.Hide();
            }
        }
    }
}