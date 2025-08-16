namespace DuelGame
{
    [System.Serializable]
    public class HeroesConfig
    {
        public HeroConfig Archer;
        public HeroConfig Wizard;
        public HeroConfig Warrior;
    }
    
    [System.Serializable]
    public class HeroConfig
    {
        public float Health = 100;
        public float Damage = 25;
        public float Armor = 1;
        public float AttackRate = 3;
    }
}