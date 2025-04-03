using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DuelGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Warrior : BaseHero
    {
        private readonly float _stunChance = 0.4f;

        private CapsuleCollider2D _attackCollider;

        private void Awake()
        {
            _bodyCollider = GetComponent<BoxCollider2D>();
            _attackCollider = GetComponent<CapsuleCollider2D>();
        }
        
        public void TurnOnColl()
        {
            _attackCollider.enabled = false;
            _attackCollider.enabled = true;
        }
        
        public void TurnOffColl()
        {
            _attackCollider.enabled = false;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(_attackCollider.enabled && 
               collision.TryGetComponent<BaseHero>(out BaseHero hero) && 
               collision.gameObject != gameObject && 
               collision.layerOverridePriority == PLAYER_LAYER)
            {
                BuffEnum buffEnum = BuffEnum.None;

                float roll = Random.value;
                if(roll <= _stunChance) 
                {
                    buffEnum = BuffEnum.Stun;
                    InvokeApplyBuffToEnemy(hero);
                }                
                hero.TakeHit(this.hero.damage, buffEnum);
            }
        }    
    }
}

