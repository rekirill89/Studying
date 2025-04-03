using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseHeroVisual : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _buffSprite;
        [SerializeField] protected BaseHero _hero;

        [SerializeField] protected DamageText _damageTextPrefab;
        [SerializeField] protected Transform _damageTextPosition;

        public Image healthBar;

        protected Animator _animator;
        
        private CancellationTokenSource _cts;

        private ObjectPool<DamageText> _damageTextPool;
        private int _initSizeOfPool = 8;
        private float _currenBuffDuration = 0;

        protected const string ATTACK = "Attack";
        private const string HIT = "Hit";
        private const string DEATH = "Death";


        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _damageTextPool = new ObjectPool<DamageText>(_damageTextPrefab, _initSizeOfPool);
            _cts = new CancellationTokenSource();
        }

        private void Start()
        {
            healthBar.fillAmount = 1f;
            SubscribeToEvents();
        }
        
        protected void HeroDeath(Players _)
        { 
            _animator.SetTrigger(DEATH);
        }
        
        protected void HeroTakeHit(Players __)
        {
            _animator.SetTrigger(HIT);
        }
        
        protected void ShowDamageText(float damage)
        {
            Debug.Log("Trrigered");

            var x = _damageTextPool.Get();
            x.Initialize(damage, _damageTextPool.ReturnToPool);

            x.transform.SetParent(_damageTextPosition);
            x.transform.localPosition = new Vector2(0, 0);
        }

        protected virtual void SubscribeToEvents()
        {
            _hero.OnTakeHit += HeroTakeHit;
            _hero.OnTakeDamage += ShowDamageText;
            _hero.OnHealthChanged += ChangeHealthBar;

            _hero.OnDeath += HeroDeath;
            _hero.OnBuffApplied += HeroBuffApplied;

            _hero.OnAttack += HeroAttack;
        }

        protected virtual void UnsubscribeFromEvents()
        {
            _hero.OnTakeHit -= HeroTakeHit;
            _hero.OnTakeDamage -= ShowDamageText;
            _hero.OnHealthChanged -= ChangeHealthBar;
            
            _hero.OnDeath -= HeroDeath;
            _hero.OnBuffApplied -= HeroBuffApplied;
            
            _hero.OnAttack -= HeroAttack;

        }
        
        protected void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
            
        protected void HeroBuffApplied(Sprite buffSprite, float duration)
        {
            var task = SpawnBuffSprite(buffSprite, duration);
        }
        
        private async UniTask SpawnBuffSprite(Sprite sp, float duration)
        {
            if (_buffSprite.enabled && _buffSprite.sprite == sp && !_cts.IsCancellationRequested)
            {
                _currenBuffDuration += duration;
                return;
            }
            
            _currenBuffDuration = duration;
            _buffSprite.enabled = true;
            _buffSprite.sprite = sp;
            
            float step = 0.1f;
            while (_currenBuffDuration > 0 && !_cts.Token.IsCancellationRequested)
            {
                _currenBuffDuration -= step;
                await UniTask.Delay(TimeSpan.FromSeconds(step), cancellationToken: _cts.Token);
            }
            _buffSprite.enabled = false;
        }
        
        private void HeroAttack()
        {
            _animator.SetTrigger(ATTACK);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
            _cts.Cancel();
        }
    }
}

