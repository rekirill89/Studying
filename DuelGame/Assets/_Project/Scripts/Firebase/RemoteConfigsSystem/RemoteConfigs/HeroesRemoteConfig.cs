namespace DuelGame
{
    [System.Serializable]
    public class HeroesRemoteConfig
    {
        public HeroRemoteConfig Archer;
        public HeroRemoteConfig Wizard;
        public HeroRemoteConfig Warrior;
    }
    
    [System.Serializable]
    public class HeroRemoteConfig
    {
        public float Health = 100;
        public float Damage = 25;
        public float Armor = 1;
        public float AttackRate = 3;
    }
}