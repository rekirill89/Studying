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

            _warrior = _hero as Warrior;
        }
        
        public void TurnOnCollAnim()
        {
            _warrior.TurnOnColl();
        }
        
        public void TurnOffCollAnim()
        {
            _warrior.TurnOffColl();
        }
    }
}

