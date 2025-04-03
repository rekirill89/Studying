using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Wizard : BaseHero
    {
        public event Action OnAttackEnded;

        private readonly float _attackDuration = 2.1f;
        private readonly float _attackInterval = 0.3f;

        private WaitForSeconds _attackIntervalTimer;

        private CapsuleCollider2D _attackCollider;
        
        private void Awake()
        {
            _attackCollider = GetComponent<CapsuleCollider2D>();
            _bodyCollider = GetComponent<BoxCollider2D>();

            _attackIntervalTimer = new WaitForSeconds(_attackInterval);
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
            if (_attackCollider.enabled && 
                collision.TryGetComponent<BaseHero>(out BaseHero hero) && 
                collision.gameObject != gameObject && 
                collision.layerOverridePriority == PLAYER_LAYER)
            {
                Debug.Log("Entered");
                StartCoroutine(PeriodicalDamage(hero));
            }
        }
        
        private IEnumerator PeriodicalDamage(BaseHero hero)
        {
            float currentAttackDuration = 0;
            _isAttackable = false;
            while (currentAttackDuration < _attackDuration)
            {            
                hero.TakeHit(this.hero.damage);
                currentAttackDuration += _attackInterval;

                yield return _attackIntervalTimer;
            }

            hero.TakeHit(this.hero.damage, BuffEnum.DecreaseDamage);
            InvokeApplyBuffToEnemy(hero);
            TurnOffColl();
            _isAttackable = true;
            OnAttackEnded?.Invoke();

            yield return null;
        }
    }
}

