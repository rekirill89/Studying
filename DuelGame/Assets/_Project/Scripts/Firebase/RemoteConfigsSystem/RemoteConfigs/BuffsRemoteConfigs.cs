namespace DuelGame
{
    [System.Serializable]
    public class BuffsRemoteConfigs
    {
        public PoisonBuffRemoteConfig Poison;
        public StunBuffRemoteConfig Stun;
        public DecreaseDamageBuffRemoteConfig DecreaseDamage;
    }
    
    [System.Serializable]
    public class PoisonBuffRemoteConfig
    {
        public float Duration = 2.1f;
        public float DamagePerTick = 3.5f;
        public float StartDelay = 0.3f;
        public float TickInterval = 0.3f;
    }
    
    [System.Serializable]
    public class StunBuffRemoteConfig
    {
        public float Duration = 4f;
    }
    
    [System.Serializable]
    public class DecreaseDamageBuffRemoteConfig
    {
        public float Duration = 5f;
        public float DamageMultiplier = 3f;
    }
}