using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class StartPanelPresenter : IPresenter<StartView>
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BaseOneButtonView _startView;

        public StartPanelPresenter(BattleManager battleManager, StartView startView)
        {
            _battleManagerModel = battleManager;
            _startView = startView;
        }
                                               
        public void Initialize()
        {
            _startView.OnButtonClicked += RunBattleHandler;
            _battleManagerModel.OnPlayersSpawned += HideView;
        }

        public void Dispose()
        {
            _startView.OnButtonClicked -= RunBattleHandler;
            _battleManagerModel.OnPlayersSpawned -= HideView;
        }

        public void ShowView()
        {
            _startView.Show();    
        }

        private void RunBattleHandler()
        {
            _battleManagerModel.RunBattle().Forget();
        }
        
        private void HideView(BattleState battleState)
        {
            if (battleState == BattleState.Started)
            {
                _startView.Hide();
            }
        }
    }   
}