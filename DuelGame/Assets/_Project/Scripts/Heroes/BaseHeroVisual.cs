using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] protected SpriteRenderer BuffSprite;
        [SerializeField] protected BaseHero Hero;

        [SerializeField] protected DamageText DamageTextPrefab;
        [SerializeField] protected Transform DamageTextPosition;

        public Image HealthBar;
        
        protected const string ATTACK = "Attack";
        protected Animator Animator;
        
        private const string HIT = "Hit";
        private const string DEATH = "Death";   
        
        private readonly List<DamageText> _activeDamageTexts = new List<DamageText>();
        private readonly int _initSizeOfPool = 8;
        
        private float _currenBuffDuration = 0;
        
        private CancellationTokenSource _cts;
        private ObjectPool<DamageText> _damageTextPool;
        
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            _damageTextPool = new ObjectPool<DamageText>(DamageTextPrefab, _initSizeOfPool);
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            HealthBar.fillAmount = 1f;
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
            foreach (var damageText in _activeDamageTexts)
                damageText.OnComplete.RemoveListener(HideDamageText);
            _cts.Cancel();
        }
        
        protected void HeroDeath(Players _)
        { 
            Animator.SetTrigger(DEATH);
        }
        
        protected void HeroTakeHit(Players __)
        {
            Animator.SetTrigger(HIT);
        }
        
        protected void ShowDamageText(float damage)
        {
            Debug.Log("Triggered");

            var x = _damageTextPool.Get();
            
            _activeDamageTexts.Add(x);
            
            x.OnComplete.AddListener(HideDamageText);
            x.Initialize(damage);
            x.transform.SetParent(DamageTextPosition);
            x.transform.localPosition = new Vector2(0, 0);
        }

        protected virtual void SubscribeToEvents()
        {
            Hero.OnTakeHit += HeroTakeHit;
            Hero.OnTakeDamage += ShowDamageText;
            Hero.OnHealthChanged += ChangeHealthBar;
            Hero.OnPlayerStop += StopAllTasks;

            Hero.OnDeath += HeroDeath;
            Hero.OnBuffApplied += HeroBuffApplied;

            Hero.OnAttack += HeroAttack;
        }

        protected virtual void UnsubscribeFromEvents()
        {
            Hero.OnTakeHit -= HeroTakeHit;
            Hero.OnTakeDamage -= ShowDamageText;
            Hero.OnHealthChanged -= ChangeHealthBar;
            Hero.OnPlayerStop -= StopAllTasks;
            
            Hero.OnDeath -= HeroDeath;
            Hero.OnBuffApplied -= HeroBuffApplied;
            
            Hero.OnAttack -= HeroAttack;
        }

        protected void StopAllTasks()
        {
            _cts.Cancel();
        }
        
        protected void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            HealthBar.fillAmount = currentHealth / maxHealth;
        }
            
        protected void HeroBuffApplied(Sprite buffSprite, float duration)
        {
            SpawnBuffSprite(buffSprite, duration).Forget();
        }
        
        private async UniTask SpawnBuffSprite(Sprite sp, float duration)
        {
            _currenBuffDuration = duration;
            BuffSprite.enabled = true;
            BuffSprite.sprite = sp;

            float step = 0.1f;
            while (_currenBuffDuration > 0 && !_cts.Token.IsCancellationRequested)
            {
                _currenBuffDuration -= step;
                await UniTask.Delay(TimeSpan.FromSeconds(step), cancellationToken: _cts.Token);
            }
            BuffSprite.enabled = false;
        }
        
        private void HideDamageText(DamageText damageText)
        {
            _damageTextPool.ReturnToPool(damageText);
            damageText.OnComplete.RemoveListener(HideDamageText);
            _activeDamageTexts.Remove(damageText);
        }

        private void HeroAttack()
        {
            Animator.SetTrigger(ATTACK);
        }
    }
}