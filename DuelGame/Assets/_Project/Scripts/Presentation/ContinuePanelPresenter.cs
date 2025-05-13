using System;
using UnityEngine;

namespace DuelGame
{
    public class ContinuePanelPresenter :IDisposable
    {
        private readonly BattleManager _battleManagerModel;
        private readonly ContinuePanelView _continuePanelView;

        public ContinuePanelPresenter(BattleManager battleManager, ContinuePanelView continuePanelView)
        {
            _battleManagerModel = battleManager;
            _continuePanelView = continuePanelView;

            _continuePanelView.OnButtonClicked += _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish += ShowView;
            _battleManagerModel.OnPlayersSpawned += HideView;
        }

        public void Dispose()
        {
            _continuePanelView.OnButtonClicked -= _battleManagerModel.ContinueBattle;
            _battleManagerModel.OnBattleFinish -= ShowView;
            _battleManagerModel.OnPlayersSpawned -= HideView;
        }
        
        private void HideView(BattleState battleState)
        {
            if (battleState == BattleState.Continued)
            {
                _continuePanelView.Hide();
            }
        }
        
        private void ShowView(Players playerWhoLost)
        {
            if (playerWhoLost == Players.Player2)
            {
                _continuePanelView.Show();
            }
        }
    }   
}
