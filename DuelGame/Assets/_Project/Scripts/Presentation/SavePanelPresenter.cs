using System;
using Zenject;

namespace DuelGame
{
    public class SavePanelPresenter : IPresenter<SavePanelView>
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BattleDataController _battleDataController;
        private readonly SavePanelView _savePanelView;

        public SavePanelPresenter(
            BattleManager battleManagerModel, 
            BattleDataController battleDataController, 
            SavePanelView savePanelView)
        {
            _battleManagerModel = battleManagerModel;
            _battleDataController = battleDataController;
            _savePanelView = savePanelView;
        }
                                               
        public void Initialize()
        {
            _savePanelView.OnButtonClicked += ButtonClickedHandler;
            _battleManagerModel.OnPlayersSpawned += ShowView;
            _battleManagerModel.OnBattleFinish += HideView;
        }

        public void Dispose()
        {
            _savePanelView.OnButtonClicked -= ButtonClickedHandler;
            _battleManagerModel.OnPlayersSpawned -= ShowView;
            _battleManagerModel.OnBattleFinish -= HideView;
        }

        public void ShowView()
        {
            _savePanelView.Show();
        }
        
        private void ShowView(BattleState _)
        {
            _savePanelView.Show();
        }
        
        private void HideView(Players? _)
        {
            _savePanelView.Hide();
        }

        private void ButtonClickedHandler()
        {
            _battleDataController.SaveBattleData(null);
        }
    }
}