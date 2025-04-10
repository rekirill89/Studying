using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class ArcherVisual : BaseHeroVisual
    {
        private Archer _archer;

        protected override void Awake()
        {
            base.Awake();

            _archer = Hero as Archer;
        }   
        
        public void SpawnArrowInAnim()
        {
            _archer.SpawnArrow();
        } 
        


    }
}

