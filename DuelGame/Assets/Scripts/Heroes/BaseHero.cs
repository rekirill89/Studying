using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public abstract class BaseHero : MonoBehaviour
    {
        public event Action<float, Players> OnTakeHit;
        public event Action<Players> OnDeath;
        public event Action OnAttack;
        public event Action<BuffEnum, BaseHero> OnBuffGot;


        public event Action OnAttackVisual;
        public event Action OnDeathVisual;        
        public event Action OnTakeHitVisual;
        public abstract Image healthBar { get; set; }

        public HeroStats hero { get; protected set; }
        public Players player { get; protected set; } = default;
        public bool isDead { get; protected set; } = false;

        protected BoxCollider2D _bodyCollider;
        protected float _currentHealth = 0;
        protected float _timeBetweenAttack = 0;
        protected bool _isAttackable = false;


        public virtual void Attack()
        {
            _timeBetweenAttack -= Time.fixedDeltaTime;
            if (_timeBetweenAttack <= 0)
            {
                OnAttack?.Invoke();
                OnAttackVisual?.Invoke();

                _timeBetweenAttack = hero.attackRate;
            }
        }
        public virtual void ChangeAttackStatus(bool isAttackable)
        {
            _isAttackable = isAttackable;
        }
        public virtual void ChangeCurrHealth(float damage)
        {
            _currentHealth -= damage;
            OnTakeHit?.Invoke(damage, player);
            ChangeHealthBar();
        }
        public virtual void GetStunned(float stunDuration)
        {
            _timeBetweenAttack += stunDuration;
        }
        public virtual void TakeHit(float damage, BuffEnum buffGot = BuffEnum.None)
        {
            float realDamage = (damage - (damage * (hero.armor / 10)));
            ChangeCurrHealth(realDamage);   
                        
            OnTakeHitVisual?.Invoke();

            OnBuffGot?.Invoke(buffGot, this);

            if (_currentHealth <= 0)
            {
                isDead = true;
                
                OnDeath?.Invoke(player);
                OnDeathVisual?.Invoke();
                TurnOffBodyCollider();
            }
        }
        public virtual void GetPlayerID(Players player)
        {
            this.player = player;
        }
        public void TurnOffBodyCollider()
        {
            _bodyCollider.enabled = false;
        }
        public virtual void StartHandler()
        {
            _currentHealth = hero.health;
            healthBar.fillAmount = 1;
        }
        public virtual void ChangeHealthBar()
        {
            //Debug.Log(_currentHealth + " " + hero.health);
            healthBar.fillAmount = _currentHealth / hero.health;
        }

        public void Initialize(HeroStats hero)
        {
            this.hero = Instantiate(hero);
        }
    }
}

