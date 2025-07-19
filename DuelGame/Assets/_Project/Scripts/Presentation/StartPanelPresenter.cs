using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class StartPanelPresenter : IDisposable, IPresenter
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonPanelView _startPanelView;

        public StartPanelPresenter(BattleManager battleManager, StartPanelView startPanelView)
        {
            _battleManagerModel = battleManager;
            _startPanelView = startPanelView;
            
            _startPanelView.OnButtonClicked += RunBattleHandler;
            _battleManagerModel.OnPlayersSpawned += HideView;
        }

        public void Dispose()
        {
            _startPanelView.OnButtonClicked -= RunBattleHandler;
            _battleManagerModel.OnPlayersSpawned -= HideView;
        }

        public void ShowView()
        {
            _startPanelView.Show();    
        }

        private void RunBattleHandler()
        {
            _battleManagerModel.RunBattle().Forget();
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