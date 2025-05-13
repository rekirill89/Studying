using System;
using UnityEngine;

namespace DuelGame
{
    public class StartPanelPresenter : IDisposable
    {
        private readonly BattleManager _battleManagerModel;
        private readonly StartPanelView _startPanelView;

        public StartPanelPresenter(BattleManager battleManager, StartPanelView startPanelView)
        {
            _battleManagerModel = battleManager;
            _startPanelView = startPanelView;

            _startPanelView.OnButtonClicked += _battleManagerModel.RunBattle;
            _battleManagerModel.OnPlayersSpawned += HideView;
        }

        public void Dispose()
        {
            _startPanelView.OnButtonClicked -= _battleManagerModel.RunBattle;
            _battleManagerModel.OnPlayersSpawned -= HideView;
        }
        
        private void HideView(BattleState battleState)
        {
            if (battleState == BattleState.Started)
            {
                _startPanelView.Hide();
            }
        }
    }   
}
