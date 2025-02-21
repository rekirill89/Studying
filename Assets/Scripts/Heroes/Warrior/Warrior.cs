using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Warrior : MonoBehaviour, IHero
{
    [SerializeField] private Image _healthBar;

    public event Action OnAttack;
    public event Action OnTakeHit;
    public event Action OnDeath;

    public bool isDead { get; set; } = false;
    public Side side { get; set; } = Side.Left;
    public Player player { get; set; } = default;

    public HeroStats hero { get; set; }

    private float _currentHealth;
    private float _timeBetweenAttack = 0f;
    private bool _isAttackable = false;

    private CapsuleCollider2D _attackCollider;
    private BoxCollider2D _bodyCollider;

    private void Awake()
    {
        _bodyCollider = GetComponent<BoxCollider2D>();
        _attackCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        hero = Instantiate(PlayersManager.Instance.heroes.GetHeroStatsByName("Warrior"));
        _currentHealth = hero.health;
        _healthBar.fillAmount = 1;
    }
    private void FixedUpdate()
    {
        if (!_isAttackable) return;
        Attack();
    }
    public void Attack()
    {
        _timeBetweenAttack -= Time.fixedDeltaTime;
        if(_timeBetweenAttack <= 0)
        {
            OnAttack?.Invoke();

            _timeBetweenAttack = hero.attackRate;
        }
    }   
    public void TakeHit(float damage)
    {
        OnTakeHit?.Invoke();

        float realDamage = (damage - (damage * (hero.armor / 10)));
        ChangeCurrHealth(realDamage);
        
        if (_currentHealth <= 0)
        {
            isDead = true;            

            OnDeath?.Invoke();
            //_bodyCollider.l = false;

            PlayersManager.Instance.FinishBattle();
        }
    }
    public void ChangeCurrHealth(float damage)
    {
        PlayersManager.Instance.TakeHitShowText(gameObject, Mathf.Round(damage));
        _currentHealth -= damage;        

        ChangeHealthBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggerd");
        if(_attackCollider.enabled && collision.TryGetComponent<IHero>(out IHero hero) && collision.gameObject != gameObject)
        {
            Debug.Log(collision.gameObject.GetHashCode());
            hero.TakeHit(this.hero.damage);

            int roll = Random.Range(0, 11);
            if(roll <= 4) 
            {
                StunBuff buff = new StunBuff();
                CorutineManager.Instance.StartCoroutine(buff.ApplyBuff(hero));
            }
        }
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

    public void ChangeAttackStatus(bool isAttackable)
    {
        _isAttackable = isAttackable;
    }

    private void ChangeHealthBar()
    {
        _healthBar.fillAmount = _currentHealth / hero.health;
    }

    public void GetStunned(float stunDuration)
    {
        _timeBetweenAttack += stunDuration;
    }
}
