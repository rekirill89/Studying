using UnityEngine;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Scriptable Objects/Hero")]
    public class HeroStats : ScriptableObject
    {
        public string heroName;

        public float health;
        public float damage;
        public float armor;
        public float attackRate;
    }
}

