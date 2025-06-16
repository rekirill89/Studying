using System.Collections;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class WarriorVisual : BaseHeroVisual
    {
        private Warrior _warrior;

        protected override void Awake()
        {
            base.Awake();

            _warrior = Hero as Warrior;
        }

        public void DamageEnemy()
        {
            _warrior.DamageEnemy();
        }
    }
}