using System;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        
        public bool isDead { get; protected set; } = false;
        public HeroStats hero { get; private set; }
        public BuffsList buffList { get; private set; }
        public UniTask currentBuffTask;
        
        protected Players _player = default;
        protected BoxCollider2D _bodyCollider;
        protected bool _isAttackable = false;
        
        protected const int PLAYER_LAYER = 0;
        private const float ARMOR_COEFFICIENT = 0.1f;
        
        private float _currenStunDuration = 0;
        private CancellationTokenSource _cts;
        private UniTask _attackTask;
        private float _currentHealth = 0;



        private void Start()
        {
            StartHandler();
            Attack();
        }
        
        public void Initialize(HeroStats hero, BuffsList buffs)
        {
            this.hero = Instantiate(hero);
            buffList = buffs;
        }
        
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
            var x = GetStunnedUntilTime(stunDuration);
        }
        
        public void TakeHit(float damage, BuffEnum buffGot = BuffEnum.None)
        {
            float realDamage = (damage - (damage * (hero.armor * ARMOR_COEFFICIENT)));
            OnTakeHit?.Invoke(_player);
            ChangeCurrentHealth(realDamage);

            Debug.Log(buffGot);
        }
        
        public void BuffAppliedInvoke(Sprite buffSprite, float buffDuration)
        {
            OnBuffApplied?.Invoke(buffSprite, buffDuration);
        }
        
        public void SetPlayerID(Players player)
        {
            this._player = player;
        }

        protected void InvokeApplyBuffToEnemy(BaseHero hero)
        {
            OnApplyBuffToEnemy?.Invoke(hero);
        }
        
        private void StartHandler()
        {
            _currentHealth = hero.health;
            
            _cts = new CancellationTokenSource();
        }
        
        private void Attack()
        {
            _attackTask = AttackTimer();
        }

        private async UniTask AttackTimer()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (!_isAttackable || _currenStunDuration > 0)
                {
                    await UniTask.DelayFrame(1);
                    continue;
                }
                OnAttack?.Invoke();
                                
                await UniTask.Delay(TimeSpan.FromSeconds(hero.attackRate), cancellationToken: _cts.Token);
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
            while (_currenStunDuration > 0)
            {
                _currenStunDuration -= step;
                await UniTask.Delay(TimeSpan.FromSeconds(step));
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
                
                OnDeath?.Invoke(_player);
                TurnOffBodyCollider();
            }
        }

        private void OnDestroy()
        {
            _cts.Cancel();
        }
    }
    public enum Players
    {
        Player1,
        Player2
    }
}

