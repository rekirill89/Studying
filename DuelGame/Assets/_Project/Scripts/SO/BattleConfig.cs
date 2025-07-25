using UnityEngine;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "Scriptable Objects/BattleConfig")]
    public class BattleConfig : ScriptableObject
    {
        [field: SerializeField] public float AttackDelayP1 {get; private set;}
        [field: SerializeField] public float AttackDelayP2 {get; private set;}
        
        [field: SerializeField] public HeroEnum FirstHero {get; private set;}
        [field: SerializeField] public HeroEnum SecondHero {get; private set;}

        public void SetRemoteConfigStats(BattleRemoteConfig remoteConfig)
        {
            AttackDelayP1 = remoteConfig.AttackDelayP1;
            AttackDelayP2 = remoteConfig.AttackDelayP2;
        }
    }
}