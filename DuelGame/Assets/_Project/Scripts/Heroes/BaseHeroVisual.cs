using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    public abstract class BaseHeroVisual : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer buffSprite;
        [SerializeField] protected BaseHero hero;

        [SerializeField] protected DamageText damageTextPrefab;
        [SerializeField] protected Transform damageTextPosition;

        [SerializeField] public Image healthBar;

        protected Animator _animator;
        private ObjectPool<DamageText> _damageTextPool;

        protected const string ATTACK = "Attack";
        private const string HIT = "Hit";
        private const string DEATH = "Death";


        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _damageTextPool = new ObjectPool<DamageText>(damageTextPrefab, 8);
        }

        protected void Start()
        {
            healthBar.fillAmount = 1f;
        }

        
        protected void HeroDeath(Players _)
        { 
            _animator.SetTrigger(DEATH);
        }
        protected void HeroTakeHit(Players __)
        {
            _animator.SetTrigger(HIT);
        }
        
        protected void HeroBuffAplied(Sprite buffSprite, float duration)
        {
            StartCoroutine(SpawnBuffSprite(buffSprite, duration));

        }
        
        protected void ShowDamageText(float damage)
        {
            Debug.Log("Trrigered");

            var x = _damageTextPool.Get();
            x.Initialize(damage, _damageTextPool.ReturnToPool);

            x.transform.SetParent(damageTextPosition);
            x.transform.localPosition = new Vector2(0, 0);
        }

        protected virtual void SubscribeToEvents()
        {
            hero.OnTakeHit += HeroTakeHit;
            hero.OnTakeDamage += ShowDamageText;
            hero.OnHealthChanged += ChangeHealthBar;

            hero.OnDeath += HeroDeath;
            hero.OnBuffAplied += HeroBuffAplied;

            hero.OnAttack += HeroAttack;
        }
        
        protected void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
            
        
        private IEnumerator SpawnBuffSprite(Sprite sp, float duration)
        {
            buffSprite.enabled = true;
            buffSprite.sprite = sp;
            Debug.Log($"Buff Sprite Spawned, duration {duration}");

            yield return new WaitForSeconds(duration); // duration is different each time, that's why I didn't cash it 

            Debug.Log($"Buff Sprite enabled");
            buffSprite.enabled = false;
        }
        
        private void HeroAttack()
        {
            _animator.SetTrigger(ATTACK);
        }
    }
}

