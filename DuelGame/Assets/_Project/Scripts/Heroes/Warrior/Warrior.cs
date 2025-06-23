using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DuelGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Warrior : BaseHero
    {
        public override HeroEnum heroEnum { get; } = HeroEnum.Warrior;

        private readonly float _stunChance = 0.4f;

        private void Awake()
        {
            BodyCollider = GetComponent<BoxCollider2D>();
        }
        
        public override void DamageEnemy()
        {
            float roll = Random.value;
            if(roll <= _stunChance) 
            {
                InvokeApplyBuffToEnemy(EnemyHero);
            }                
            EnemyHero.TakeHit(Hero.Damage);
        }
    }
}