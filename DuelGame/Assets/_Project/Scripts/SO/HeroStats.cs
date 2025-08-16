using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Scriptable Objects/Hero")]
    public class HeroStats : ScriptableObject
    {
        public string HeroName;

        public float Health;
        public float Damage;
        public float Armor;
        public float AttackRate;

        public void UpdateStats(HeroConfig hero)
        {
            Health = hero.Health;
            Damage = hero.Damage;
            Armor = hero.Armor;
            AttackRate = hero.AttackRate;
        }
    }
}