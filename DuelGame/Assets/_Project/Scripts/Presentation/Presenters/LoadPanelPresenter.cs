using System;
using Cysharp.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class LoadPanelPresenter : IPresenter<LoadView>
    {
        private readonly BattleDataController _battleDataController;
        private readonly SaveService _saveService;
        private readonly UnityCloudSaveService _unityCloudSaveService;
        private readonly LoadView _loadView;
        private readonly ChooseSaveView _chooseSaveView;
        
        private readonly InternetConnector _internetConnector;
        
        private UserData _localUserData;
        private UserData _cloudUserData;

        public LoadPanelPresenter(
            BattleDataController battleDataController, 
            SaveService saveService,
            UnityCloudSaveService unityCloudSaveService,
            LoadView loadView,
            InternetConnector internetConnector)
        {
            _battleDataController = battleDataController;
            _saveService = saveService;
            _unityCloudSaveService = unityCloudSaveService;
            
            _loadView = loadView;
            _chooseSaveView = _loadView.ChooseSaveView;
            
            _internetConnector = internetConnector;
        }
        
        public void Initialize()
        {
            _loadView.LoadDataButton.OnClick += OnLoadDataButtonClickHandler;
            
            _chooseSaveView.LoadAutoSaveButton.OnClick += OnAutoSaveButtonClickHandler;
            _chooseSaveView.LoadCloudSaveButton.OnClick += OnCloudSaveButtonClickHandler;
        }

        public void Dispose()
        {
            _loadView.LoadDataButton.OnClick -= OnLoadDataButtonClickHandler;
            
            _chooseSaveView.LoadAutoSaveButton.OnClick -= OnAutoSaveButtonClickHandler;
            _chooseSaveView.LoadCloudSaveButton.OnClick -= OnCloudSaveButtonClickHandler;
        }
        
        public void ShowView()
        {
            _loadView.Show();
        }

        private void HideView(BattleState _)
        {
            _loadView.Hide();
        }

        private void OnLoadDataButtonClickHandler()
        {
            OnLoadDataButtonClickHandlerAsync().Forget();
        }
        
        private async UniTask OnLoadDataButtonClickHandlerAsync()
        {
            if (_internetConnector.IsConnected)
            {
                _localUserData = _saveService.Load();
                _cloudUserData = await _unityCloudSaveService.LoadUserDataAsync();
                
                if (DateTime.Parse(_cloudUserData.SaveTime) < DateTime.Parse(_localUserData.SaveTime))
                {
                    _chooseSaveView.Show();
                }
                else
                {
                    _battleDataController.LoadBattleDataAsync().Forget();
                }
            }
            else
            {
                _battleDataController.LoadBattleData();
            }
        }

        private void OnCloudSaveButtonClickHandler()
        {
            _saveService.Save(_cloudUserData);
            _battleDataController.LoadBattleDataAsync().Forget();
        }

        private void OnAutoSaveButtonClickHandler()
        {
            _unityCloudSaveService.SaveUserDataAsync(_localUserData).Forget();
            _battleDataController.LoadBattleData();
        }
    }
}