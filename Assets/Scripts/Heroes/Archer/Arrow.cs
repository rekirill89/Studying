using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float _timerToDestroy = 8f;
    private float _damage = 0f;
    private float _speed = 15f;

    private Buff _buffToApply;

    private void Update()
    {
        _timerToDestroy -= Time.deltaTime;

        if (_timerToDestroy <= 0)
            Destroy(gameObject);
    }
    void FixedUpdate()
    {
        transform.position += transform.right * _speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHero>(out IHero hero))
        {
            hero.TakeHit(_damage);

            CorutineManager.Instance.StartCoroutine(_buffToApply.ApplyBuff(hero));
            Destroy(gameObject);
        }
    }

    public void Initialize(float damage, Buff buffToApply)
    {
        this._buffToApply = buffToApply;
        _damage = damage;
    }
}
