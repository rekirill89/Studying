using UnityEngine;

namespace DuelGame
{
    public class BattleSessionContext
    {
        public BattleData BattleData {get;}
        
        public float AttackDelayP1 {get; private set;}
        public float AttackDelayP2 {get; private set;}
        
        public Transform FirstPlayerTrans {get; private set;}
        public Transform SecondPlayerTrans {get; private set;}
        
        public BattleSessionContext(BattleDataCache battleDataCache, BattleSettingsFacade facade)
        {
            FirstPlayerTrans = facade.FirstPlayerTrans;
            SecondPlayerTrans = facade.SecondPlayerTrans;

            AttackDelayP1 = facade.BattleConfig.AttackDelayP1;
            AttackDelayP2 = facade.BattleConfig.AttackDelayP2;

            var battleData = battleDataCache.ConsumeBattleData();
            
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
        }
    }
}