using System;
using UnityEngine;

namespace DuelGame
{
    public class BattleSessionContext : IDisposable
    {
        public event Action OnSessionReady;
        
        public BattleData BattleData {get; private set; }
        public float AttackDelayP1 {get; private set;}
        public float AttackDelayP2 {get; private set;}
        
        public Transform FirstPlayerTrans {get; private set;}
        public Transform SecondPlayerTrans {get; private set;}

        public bool IsSystemReady { get; private set; } = false;

        private readonly BattleSettingsFacade _facade;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader;
        private readonly BattleDataCache _battleDataCache;
        
        public BattleSessionContext(BattleDataCache battleDataCache, BattleSceneAssetsLoader battleSceneAssetsLoader)
        {
            Debug.Log("Context started");
            _battleDataCache = battleDataCache;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;

            _battleSceneAssetsLoader.OnBattleSceneAssetsReady += Init;
        }

        public void Dispose()
        {
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady -= Init;
        }
        
        private void Init(BattleSettingsFacade facade, Panels _)
        {
            FirstPlayerTrans = facade.FirstPlayerTrans;
            SecondPlayerTrans = facade.SecondPlayerTrans;
            
            AttackDelayP1 = facade.BattleConfig.AttackDelayP1;
            AttackDelayP2 = facade.BattleConfig.AttackDelayP2;

            var battleData = _battleDataCache.ConsumeBattleData();
            
            if (battleData == null)
            {
                BattleData = new BattleData()
                {
                    Player1 = facade.BattleConfig.FirstHero,
                    Player2 = facade.BattleConfig.SecondHero,
                };
            }
            else
            {
                BattleData = battleData;
            }
            OnSessionReady?.Invoke();
            IsSystemReady = true;
        }
    }
}