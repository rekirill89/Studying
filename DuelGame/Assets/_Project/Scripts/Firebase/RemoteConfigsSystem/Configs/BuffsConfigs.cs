namespace DuelGame
{
    [System.Serializable]
    public class BuffsConfigs
    {
        public PoisonBuffConfig Poison;
        public StunBuffConfig Stun;
        public DecreaseDamageBuffConfig DecreaseDamage;
    }
    
    [System.Serializable]
    public class PoisonBuffConfig
    {
        public float Duration = 2.1f;
        public float DamagePerTick = 3.5f;
        public float StartDelay = 0.3f;
        public float TickInterval = 0.3f;
    }
    
    [System.Serializable]
    public class StunBuffConfig
    {
        public float Duration = 4f;
    }
    
    [System.Serializable]
    public class DecreaseDamageBuffConfig
    {
        public float Duration = 5f;
        public float DamageMultiplier = 3f;
    }
}