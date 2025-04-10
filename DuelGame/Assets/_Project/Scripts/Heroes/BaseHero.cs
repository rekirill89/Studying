using System;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public delegate void PlayerDeath(Players player);
    public abstract class BaseHero : MonoBehaviour
    {
        public delegate void PlayerTakeHit(Players players);
        public delegate void TakeDamage(float damage);
        public delegate void ApplyBuff(Sprite sprite, float duration);
        public delegate void PlayerHealthChanged(float currentHealth, float maxHealth);
        public delegate void ApplyBuffToEnemy(BaseHero hero);
        
        public event PlayerTakeHit OnTakeHit;
        public event TakeDamage OnTakeDamage;
        public event PlayerDeath OnDeath;
        public event PlayerHealthChanged OnHealthChanged;
        public event ApplyBuffToEnemy OnApplyBuffToEnemy;
        public event Action OnAttack;
        public event ApplyBuff OnBuffApplied;
        public event Action OnPlayerStop;

        public HeroStats Hero { get; private set; }
        public BuffsList BuffList { get; private set; }
        
        protected BaseHero EnemyHero;
        protected Players Player = default;
        protected BoxCollider2D BodyCollider;
        
        protected bool IsAttackable = false;
        protected CancellationTokenSource Cts;
        
        private bool isDead { get; set; } = false;
        private const float ARMOR_COEFFICIENT = 0.1f;
        private float _currenStunDuration = 0;
        private float _currentHealth = 0;
        
        private void Start()
        {
            StartHandler();
            Attack();
        }
        
        private void OnDestroy()
        {
            Cts.Cancel();
        }
        
        public void Initialize(HeroStats hero, BuffsList buffs)
        {
            this.Hero = Instantiate(hero);
            BuffList = buffs;
        }
        
        public void ChangeAttackStatus(bool isAttackable)
        {
            IsAttackable = isAttackable;
        }
        
        public void ChangeCurrentHealth(float damage)
        {
            if(isDead)
                return;

            _currentHealth -= damage;
            OnTakeDamage?.Invoke(damage);
            OnHealthChanged?.Invoke(_currentHealth, Hero.Health);
            
            CheckDeath();
        }
        
        public void GetStunned(float stunDuration)
        {
            GetStunnedUntilTime(stunDuration).Forget();
        }
        
        public void TakeHit(float damage)
        {
            if(isDead)
                return;
            
            float realDamage = (damage - (damage * (Hero.Armor * ARMOR_COEFFICIENT)));
            OnTakeHit?.Invoke(Player);
            ChangeCurrentHealth(realDamage);
        }
        
        public void BuffAppliedInvoke(Sprite buffSprite, float buffDuration)
        {
            OnBuffApplied?.Invoke(buffSprite, buffDuration);
        }
        
        public void SetPlayerID(Players player)
        {
            this.Player = player;
        }

        public void SetEnemy(BaseHero enemy)
        {
            EnemyHero = enemy;
        }

        public virtual void DamageEnemy()
        {
            EnemyHero.TakeHit(Hero.Damage);
            InvokeApplyBuffToEnemy(EnemyHero);
        }

        public void StopAllTasks()
        {
            Cts.Cancel();
            OnPlayerStop?.Invoke();
        }

        protected void InvokeApplyBuffToEnemy(BaseHero hero)
        {
            OnApplyBuffToEnemy?.Invoke(hero);
        }
        
        private void StartHandler()
        {
            Cts = new CancellationTokenSource();
            _currentHealth = Hero.Health;
        }
        
        private void Attack()
        {
            AttackTimer().Forget();
        }

        private async UniTask AttackTimer()
        {
            try
            {
                while (!Cts.IsCancellationRequested)
                {
                    if (!IsAttackable || _currenStunDuration > 0)
                    {
                        await UniTask.DelayFrame(1);
                        continue;
                    }

                    OnAttack?.Invoke();

                    await UniTask.Delay(TimeSpan.FromSeconds(Hero.AttackRate), cancellationToken: Cts.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private async UniTask GetStunnedUntilTime(float stunDuration)
        {
            try
            {
                if (_currenStunDuration > 0)
                {
                    _currenStunDuration += stunDuration;
                    return;
                }
            
                _currenStunDuration = stunDuration;
            
                float step = 0.1f;
                while (_currenStunDuration > 0 && !Cts.IsCancellationRequested)
                {
                    _currenStunDuration -= step;
                    _currenStunDuration = Mathf.Clamp(_currenStunDuration, 0, stunDuration);
                    await UniTask.Delay(TimeSpan.FromSeconds(step), cancellationToken:Cts.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private void TurnOffBodyCollider()
        {
            BodyCollider.enabled = false;
        }
        
        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                isDead = true;
                
                OnDeath?.Invoke(Player);
                TurnOffBodyCollider();
            }
        }
    }
    
    public enum Players
    {
        Player1,
        Player2
    }
}

