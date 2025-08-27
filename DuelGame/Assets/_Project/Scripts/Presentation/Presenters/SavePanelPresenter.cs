using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class SavePanelPresenter : IPresenter<SaveView>
    {
        private readonly BattleManager _battleManagerModel;
        private readonly BattleDataController _battleDataController;
        private readonly SaveView _saveView;

        public SavePanelPresenter(
            BattleManager battleManagerModel, 
            BattleDataController battleDataController, 
            SaveView saveView)
        {
            _battleManagerModel = battleManagerModel;
            _battleDataController = battleDataController;
            _saveView = saveView;
        }
                                               
        public void Initialize()
        {
            _saveView.OnButtonClicked += ButtonClickedHandler;
            _battleManagerModel.OnPlayersSpawned += ShowView;
            _battleManagerModel.OnBattleFinish += HideView;
        }

        public void Dispose()
        {
            _saveView.OnButtonClicked -= ButtonClickedHandler;
            _battleManagerModel.OnPlayersSpawned -= ShowView;
            _battleManagerModel.OnBattleFinish -= HideView;
        }

        public void ShowView()
        {
            _saveView.Show();
        }
        
        private void ShowView(BattleState _)
        {
            _saveView.Show();
        }
        
        private void HideView(Players? _)
        {
            _saveView.Hide();
        }

        private void ButtonClickedHandler()
        {
            Debug.Log("Data saved");
            _battleDataController.SaveBattleData(null);
        }
    }
}