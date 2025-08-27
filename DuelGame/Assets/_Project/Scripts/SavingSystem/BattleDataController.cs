using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BattleDataController : IDisposable, IInitializable
    {
        
        private readonly SaveService _saveService;
        private readonly BattleManager _battleManager;
        private readonly DataCache _dataCache;
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly UnityCloudSaveService _unityCloudSaveService;
        
        
        public BattleDataController(
            SaveService saveService, 
            UnityCloudSaveService unityCloudSaveService,
            BattleManager battleManager, 
            DataCache dataCache, 
            SceneLoaderService sceneLoaderService)
        {
            _saveService = saveService;
            _unityCloudSaveService = unityCloudSaveService;
            _battleManager = battleManager;
            _dataCache = dataCache;
            _sceneLoaderService = sceneLoaderService;
        }

        public void Initialize()
        {
            _battleManager.OnBattleFinish += SaveBattleData;
        }
        
        public void Dispose()
        {
            _battleManager.OnBattleFinish -= SaveBattleData;
        }
        
        public void SaveBattleData(Players? playerWhoLost)
        {
            var battleData = _battleManager.CollectBattleData(playerWhoLost);
            var userData = new UserData()
            {
                BattleData = battleData,
                IsAdsRemoved = _dataCache.IsAdsRemoved,
            };
            _saveService.Save(userData);
            _unityCloudSaveService.SaveUserDataAsync(userData).Forget();
        }

        public void LoadBattleData()
        {
            var battleData = _saveService.Load().BattleData;
            Debug.Log(battleData == null ? "No battle data" : battleData.ToString());
            Debug.Log(_saveService.Load().IsAdsRemoved);

            if (battleData == null)
            {
                Debug.LogWarning("Nothing to load");
                return;
            }
            
            _dataCache.SetBattleData(battleData);
            _dataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }

        public async UniTask LoadBattleDataAsync()
        {
            var userData = await _unityCloudSaveService.LoadUserDataAsync();
            
            if (userData.BattleData == null)
            {
                Debug.LogWarning("Nothing to load");
                return;
            }
            
            _dataCache.SetBattleData(userData.BattleData);
            _dataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }
    }
    
    [System.Serializable]
    public class BattleData
    {
        public HeroEnum Player1;
        public HeroEnum Player2;

        public Players? PlayerWhoWon = Players.None;
    }

    [System.Serializable]
    public class UserData
    {
        public BattleData BattleData;
        public bool IsAdsRemoved = false;
        public string SaveTime;
    }
}