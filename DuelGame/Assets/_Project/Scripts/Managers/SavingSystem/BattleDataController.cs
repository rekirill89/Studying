using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuelGame
{
    public class BattleDataController : IDisposable
    {
        private static string SAVE_AUTO = "Auto save";
        private static string SAVE_MANUAL = "Manual save";
        
        private readonly SaveService _saveService;
        private readonly BattleManager _battleManager;
        private readonly BattleDataCache _battleDataCache;
        private readonly SceneLoaderService _sceneLoaderService;
        
        public BattleDataController(
            SaveService saveService, 
            BattleManager battleManager, 
            BattleDataCache battleDataCache, 
            SceneLoaderService sceneLoaderService)
        {
            _saveService = saveService;
            _battleManager = battleManager;
            _battleDataCache = battleDataCache;
            _sceneLoaderService = sceneLoaderService;

            _battleManager.OnBattleFinish += AutoSaveBattleData;
        }

        public void Dispose()
        {
            _battleManager.OnBattleFinish -= AutoSaveBattleData;
        }
        
        public void ManualSaveBattleData()
        {
            _saveService.SaveData(SAVE_MANUAL, _battleManager.CollectBattleData());
        }

        public void LoadManualSaveBattleData()
        {
            var battleData = _saveService.LoadData<BattleData>(SAVE_MANUAL);
            
            _battleDataCache.SetBattleData(battleData);
            _battleDataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }

        public void LoadAutoSaveBattleData()
        {
            var battleData = _saveService.LoadData<BattleData>(SAVE_AUTO);
            
            _battleDataCache.SetBattleData(battleData);
            _battleDataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }
        
        private void AutoSaveBattleData(Players playerWhoLost)
        {
            var x = _battleManager.CollectBattleData(playerWhoLost);
            Debug.Log(x.Player1 + " " + x.Player2 + " " + x.PlayerWhoWon);
            
            _saveService.SaveData(SAVE_AUTO, x);
        }
    }
    
    public class BattleData
    {
        public HeroEnum Player1;
        public HeroEnum Player2;

        public Players PlayerWhoWon = Players.None;
    }
}