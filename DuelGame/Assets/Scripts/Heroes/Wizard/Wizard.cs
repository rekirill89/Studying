using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class Wizard : BaseHero
    {
        [field: SerializeField] public override Image healthBar { get; set; }

        public event Action OnAttackEnded;

        private float _attackDuration = 2.1f;
        private float _attackInterval = 0.3f;

        private CapsuleCollider2D _attackCollider;
        private void Awake()
        {
            _attackCollider = GetComponent<CapsuleCollider2D>();
            _bodyCollider = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            StartHandler();
        }
        private void FixedUpdate()
        {
            if (!_isAttackable) return;
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
            if (_attackCollider.enabled && collision.TryGetComponent<BaseHero>(out BaseHero hero) && collision.gameObject != gameObject)
            {
                Debug.Log("Entered");
                StartCoroutine(PeriodicalDamage(hero));
            }
        }
        private IEnumerator PeriodicalDamage(BaseHero hero)
        {
            float _currentAttackDuration = 0;
            _isAttackable = false;
            while (_currentAttackDuration < _attackDuration)
            {            
                hero.TakeHit(this.hero.damage);
                _currentAttackDuration += _attackInterval;

                yield return new WaitForSeconds(_attackInterval);
            }

            hero.TakeHit(this.hero.damage, BuffEnum.DecreaseDamage);
            TurnOffColl();
            _isAttackable = true;
            OnAttackEnded?.Invoke();

            yield return null;
        }
    }
}

