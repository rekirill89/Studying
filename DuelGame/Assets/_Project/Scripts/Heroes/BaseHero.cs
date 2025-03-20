using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public abstract class BaseHero : MonoBehaviour
    {
        public delegate void PlayerTakeHit(Players players);
        public delegate void TakeDamage(float damage);
        public delegate void PlayerDeath(Players player);
        public delegate void PlayerGotBuff(BuffEnum buff, BaseHero player);
        public delegate void BuffApply(Sprite sprite, float duration);
        public delegate void PlayerHealthChanged(float currentHealth, float maxHealth);
        
        public event PlayerTakeHit OnTakeHit;
        public event TakeDamage OnTakeDamage;
        public event PlayerDeath OnDeath;
        public event PlayerHealthChanged OnHealthChanged;

        public event Action OnAttack;

        public event PlayerGotBuff OnBuffGot;
        public event BuffApply OnBuffAplied;
        
        //public abstract Image healthBar { get; set; }
        public bool isDead { get; protected set; } = false;
        public HeroStats hero { get; private set; }
        
        protected Players player = default;
        protected BoxCollider2D _bodyCollider;
        protected bool _isAttackable = false;
        
        private float _currentHealth = 0;
        private float _timeBetweenAttack = 0;
        
        
        public void ChangeAttackStatus(bool isAttackable)
        {
            _isAttackable = isAttackable;
        }
        
        public void ChangeCurrentHealth(float damage)
        {
            _currentHealth -= damage;
            OnTakeDamage?.Invoke(damage);
            OnHealthChanged?.Invoke(_currentHealth, hero.health);
            
            CheckDeath();
        }
        
        public void GetStunned(float stunDuration)
        {
            _timeBetweenAttack += stunDuration;
        }
        
        public void TakeHit(float damage, BuffEnum buffGot = BuffEnum.None)
        {
            float realDamage = (damage - (damage * (hero.armor / 10)));
            OnTakeHit?.Invoke(player);
            ChangeCurrentHealth(realDamage);

            Debug.Log(buffGot);
            OnBuffGot?.Invoke(buffGot, this);
        }
        
        public void Initialize(HeroStats hero)
        {
            this.hero = Instantiate(hero);
        }
        
        public void BuffApliedInvoke(Sprite buffSprite, float buffDuration)
        {
            OnBuffAplied?.Invoke(buffSprite, buffDuration);
        }
        
        public void SetPlayerID(Players player)
        {
            this.player = player;
        }
        
        
        protected void StartHandler()
        {
            _currentHealth = hero.health;
        }
        protected void Attack()
        {
            _timeBetweenAttack -= Time.deltaTime;
            if (_timeBetweenAttack <= 0)
            {
                OnAttack?.Invoke();

                _timeBetweenAttack = hero.attackRate;
            }
        }
        
        
        private void TurnOffBodyCollider()
        {
            _bodyCollider.enabled = false;
        }
        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                isDead = true;
                
                OnDeath?.Invoke(player);
                TurnOffBodyCollider();
            }
        }
        
    }
}

