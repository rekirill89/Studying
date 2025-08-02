using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class LoadPanelPresenter : IPresenter<LoadPanelView>
    {
        private readonly BattleDataController _battleDataController;
        private readonly LoadPanelView _loadPanelView;

        public LoadPanelPresenter(BattleDataController battleDataController , LoadPanelView loadPanelView)
        {
            _battleDataController = battleDataController;
            _loadPanelView = loadPanelView;
        }
        
        public void Initialize()
        {
            _loadPanelView.LoadDataButton.OnClick += _battleDataController.LoadBattleData;
        }

        public void Dispose()
        {
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
    }
}