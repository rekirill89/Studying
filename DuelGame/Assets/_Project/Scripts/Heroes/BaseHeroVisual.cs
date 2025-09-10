using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseHeroVisual : MonoBehaviour
    {
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

            if(Hero.SkinAoc != null)
                Animator.runtimeAnimatorController = Hero.SkinAoc;
            
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
            foreach (var damageText in _activeDamageTexts)
                damageText.OnComplete -= HideDamageText;
            _cts.Cancel();
        }
        
        protected void HeroDeath(BaseHero _, Players __)
        { 
            Animator.SetTrigger(DEATH);
        }
        
        protected void DamageTaken(BaseHero _, float damage, float currentHealth, float maxHealth, bool isPhysicalDamage)
        {
            ShowDamageText(damage);
            ChangeHealthBar(currentHealth, maxHealth);
            if(isPhysicalDamage)
                HeroTakePhysicalHit();
        }

        protected virtual void SubscribeToEvents()
        {
            Hero.OnTakeDamage += DamageTaken;
            Hero.OnPlayerStop += StopAllTasks;

            Hero.OnDeath += HeroDeath;
            Hero.OnAttack += HeroAttack;
        }

        protected virtual void UnsubscribeFromEvents()
        {
            Hero.OnTakeDamage += DamageTaken;
            Hero.OnPlayerStop -= StopAllTasks;
            
            Hero.OnDeath -= HeroDeath;
            Hero.OnAttack -= HeroAttack;
        }

        protected void StopAllTasks()
        {
            _cts.Cancel();
        }

        private void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            HealthBar.fillAmount = currentHealth / maxHealth;
        }
        
        private void HeroTakePhysicalHit()
        {
            Animator.SetTrigger(HIT);
        }

        private void ShowDamageText(float damage)
        {
            var x = _damageTextPool.Get();
            
            _activeDamageTexts.Add(x);
            
            x.OnComplete += HideDamageText;
            x.Initialize(damage);
            x.transform.SetParent(DamageTextPosition);
            x.transform.localPosition = new Vector2(0, 0);
        }
        
        private void HideDamageText(DamageText damageText)
        {
            _damageTextPool.ReturnToPool(damageText);
            damageText.OnComplete -= HideDamageText;
            _activeDamageTexts.Remove(damageText);
        }

        private void HeroAttack(BaseHero _)
        {
            Animator.SetTrigger(ATTACK);
        }
    }
}