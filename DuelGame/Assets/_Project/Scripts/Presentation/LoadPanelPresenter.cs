using System;

namespace DuelGame
{
    public class LoadPanelPresenter : IDisposable, IPresenter
    {
        private readonly BattleManager _battleManager;
        private readonly BattleDataController _battleDataController;
        private readonly LoadPanelView _loadPanelView;

        public LoadPanelPresenter(BattleManager battleManager,BattleDataController battleDataController , LoadPanelView loadPanelView)
        {
            _battleManager = battleManager;
            _battleDataController = battleDataController;
            _loadPanelView = loadPanelView;

            _battleManager.OnBattleFinish += ShowView;
            _battleManager.OnPlayersSpawned += HideView;
            _loadPanelView.LoadAutoSaveButton.OnClick += _battleDataController.LoadAutoSaveBattleData;
            _loadPanelView.LoadManualSaveButton.OnClick += _battleDataController.LoadManualSaveBattleData;
        }

        public void Dispose()
        {
            _battleManager.OnBattleFinish -= ShowView;
            _battleManager.OnPlayersSpawned -= HideView;
            _loadPanelView.LoadAutoSaveButton.OnClick -= _battleDataController.LoadAutoSaveBattleData;
            _loadPanelView.LoadManualSaveButton.OnClick -= _battleDataController.LoadManualSaveBattleData;
        }
        
        public void ShowView(Players? _)
        {
            _loadPanelView.Show();
        }

        private void HideView(BattleState _)
        {
            _loadPanelView.Hide();
        }
    }
}