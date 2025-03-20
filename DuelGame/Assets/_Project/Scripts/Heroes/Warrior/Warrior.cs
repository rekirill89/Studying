using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DuelGame
{
    public class Warrior : BaseHero
    {
        private readonly float _stunChance = 40;

        private CapsuleCollider2D _attackCollider;

        private void Awake()
        {
            _bodyCollider = GetComponent<BoxCollider2D>();
            _attackCollider = GetComponent<CapsuleCollider2D>();
        }
        
        private void Start()
        {
            StartHandler();
        }
        
        private void Update()
        {
            if (!_isAttackable)
                return;
            Attack();
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
            if(_attackCollider.enabled && collision.TryGetComponent<BaseHero>(out BaseHero hero) && collision.gameObject != gameObject)
            {
                BuffEnum buffEnum = BuffEnum.None;

                int roll = Random.Range(0, 101);
                if(roll <= _stunChance) 
                {
                    buffEnum = BuffEnum.Stun;
                }                
                hero.TakeHit(this.hero.damage, buffEnum);
            }
        }    }
}

