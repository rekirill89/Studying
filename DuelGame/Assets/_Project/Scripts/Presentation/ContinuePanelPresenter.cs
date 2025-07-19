using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class ContinuePanelPresenter : IDisposable, IPresenter
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonPanelView _continuePanelView;

        public ContinuePanelPresenter(BattleManager battleManager, ContinuePanelView continuePanelView)
        {
            _battleManagerModel = battleManager;
            _continuePanelView = continuePanelView;
            
            _continuePanelView.OnButtonClicked += _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish += ShowView;
            _battleManagerModel.OnPlayersSpawned += HideView;
            _battleManagerModel.BattleStateModel.OnStateChanged += StateChangedHandler; 
        }
        
        public void Dispose()
        {
            _continuePanelView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
            _battleManagerModel.OnPlayersSpawned -= HideView;
            _battleManagerModel.BattleStateModel.OnStateChanged -= StateChangedHandler; 
        }

        public void ShowView(Players? playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                _continuePanelView.Show();
            }
        }
        
        private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _continuePanelView.Hide();
        }
        
        private void HideView(BattleState battleState)
        {
            if (battleState == BattleState.Continued)
            {
                _continuePanelView.Hide();
            }
        }
    }
}