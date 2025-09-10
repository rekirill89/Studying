using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public delegate void PlayerDeath(BaseHero heroWhoDie, Players player);
    public abstract class BaseHero : MonoBehaviour
    {
        public delegate void TakeDamage(BaseHero heroWhoTakeHit, float damage, float currentHealth, float maxHealth, bool isPhysicalDamage);
        public delegate void ApplyBuffToEnemy(BaseHero hero);
        public delegate void ReceiveBuff(BuffEnum buffEnum);
        public delegate void AttackEvent(BaseHero heroWhoAttack);
        
        public event TakeDamage OnTakeDamage;
        public event PlayerDeath OnDeath;
        public event AttackEvent OnAttack;
        public event Action OnPlayerStop;
        public event ReceiveBuff OnReceiveBuff;        
        public event ApplyBuffToEnemy OnApplyBuffToEnemy;
        
        public AnimatorOverrideController SkinAoc { get; private set; }
        public Dictionary<BuffEnum, Func<Buff>> BuffsDictionary {get; private set;}
        
        public HeroStats Hero { get; private set; }
        public BuffsList BuffList { get; private set; }
        public abstract HeroEnum HeroEnum { get; }

        protected BaseHero EnemyHero;
        protected Players Player = default;
        protected BoxCollider2D BodyCollider;
        
        protected bool IsAttackable = false;
        protected CancellationTokenSource Cts;
        
        private const float ARMOR_COEFFICIENT = 0.1f;
        private bool isDead { get; set; } = false;
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
        
        public void Initialize(
            HeroStats hero, 
            BuffsList buffs, 
            Dictionary<BuffEnum, Func<Buff>> buffsDictionary,
            AnimatorOverrideController skinAoc = null)
        {
            Hero = Instantiate(hero);
            BuffList = buffs;
            BuffsDictionary = buffsDictionary;
            SkinAoc = skinAoc;
            
            Debug.Log("BaseHero.Initialize");
         }
        
        public void ChangeAttackStatus(bool isAttackable)
        {
            IsAttackable = isAttackable;
        }
        
        public void TakeHit(float damage, bool isPhysicalDamage)
        {
            if(isDead)
                return;

            float realDamage = (damage - (damage * (Hero.Armor * ARMOR_COEFFICIENT)));
            _currentHealth -= realDamage;
            OnTakeDamage?.Invoke(this,damage, _currentHealth, Hero.Health, isPhysicalDamage);
            
            CheckDeath();
        }
        
        public void GetStunned(float stunDuration)
        {
            GetStunnedUntilTime(stunDuration).Forget();
        }

        public void BuffReceivedInvoke(BuffEnum buffEnum)
        {
            OnReceiveBuff?.Invoke(buffEnum);
        }
        
        public void SetPlayerID(Players player)
        {
            Player = player;
        }

        public void SetEnemy(BaseHero enemy)
        {
            EnemyHero = enemy;
        }

        public virtual void DamageEnemy()
        {
            EnemyHero.TakeHit(Hero.Damage, true);
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
            while (!Cts.IsCancellationRequested)
            {
                if (!IsAttackable || _currenStunDuration > 0)
                {
                    await UniTask.DelayFrame(1, cancellationToken: Cts.Token);
                    continue;
                }

                OnAttack?.Invoke(this);

                await UniTask.Delay(TimeSpan.FromSeconds(Hero.AttackRate), cancellationToken: Cts.Token);
            }
        }

        private async UniTask GetStunnedUntilTime(float stunDuration)
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
        
        private void TurnOffBodyCollider()
        {
            BodyCollider.enabled = false;
        }
        
        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                isDead = true;
                
                OnDeath?.Invoke(this, Player);
                TurnOffBodyCollider();
            }
        }
    }
    
    public enum Players
    {
        Player1,
        Player2,
        None
    }
}