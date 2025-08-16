using UnityEngine.Serialization;

namespace DuelGame
{
    [System.Serializable]
    public class GameConfig
    {
        public HeroesConfig Heroes;
        public BuffsConfigs Buffs;
        public BattleStatsConfig Battle;
    }
}