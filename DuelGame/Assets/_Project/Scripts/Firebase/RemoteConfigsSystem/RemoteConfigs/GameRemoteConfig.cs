using UnityEngine.Serialization;

namespace DuelGame
{
    [System.Serializable]
    public class GameRemoteConfig
    {
        public HeroesRemoteConfig Heroes;
        public BuffsRemoteConfigs Buffs;
        public BattleRemoteConfig Battle;
    }
}