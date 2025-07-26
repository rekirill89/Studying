using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class LoadPanelPresenter : IPresenter<LoadPanelView>
    {
        private readonly BattleManager _battleManager;
        private readonly BattleDataController _battleDataController;
        private readonly LoadPanelView _loadPanelView;

        public LoadPanelPresenter(BattleManager battleManager,BattleDataController battleDataController , LoadPanelView loadPanelView)
        {
            _battleManager = battleManager;
            _battleDataController = battleDataController;
            _loadPanelView = loadPanelView;
        }
        
        public void Initialize()
        {
            //_battleManager.OnBattleFinish += ShowView;
            //_battleManager.OnPlayersSpawned += HideView;
            //_battleManager.BattleStateModel.OnStateChanged += StateChangedHandler;
            //_battleManager.OnBattleReady += ShowView;
            _loadPanelView.LoadDataButton.OnClick += _battleDataController.LoadBattleData;
        }

        public void Dispose()
        {
            //_battleManager.OnBattleFinish -= ShowView;
            //_battleManager.OnPlayersSpawned -= HideView;
            //_battleManager.BattleStateModel.OnStateChanged -= StateChangedHandler;
            //_battleManager.OnBattleReady -= ShowView;
            _loadPanelView.LoadDataButton.OnClick -= _battleDataController.LoadBattleData;
        }
        
        public void ShowView()
        {
            _loadPanelView.Show();
        }

        private void HideView(BattleState _)
        {
            _loadPanelView.Hide();
        }
                
        /*private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _loadPanelView.Hide();
        }*/
    }
}