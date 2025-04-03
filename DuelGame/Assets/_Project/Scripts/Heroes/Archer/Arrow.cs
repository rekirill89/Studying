using System;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        public delegate void InvokeApplyBuffToEnemy(BaseHero hero);

        private InvokeApplyBuffToEnemy _invokeApplyBuffToEnemy = null;
        private float _timerToDestroy = 5f;
        private float _damage = 0f;
        private readonly float _speed = 15f;

        private Rigidbody2D _rb;
        private Vector2 _direction = Vector2.right;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _timerToDestroy -= Time.deltaTime;

            if (_timerToDestroy <= 0)
                Destroy(gameObject);
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * _speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<BaseHero>(out BaseHero hero) && collision.layerOverridePriority == 0)
            {
                hero.TakeHit(_damage, BuffEnum.Poison);
                _invokeApplyBuffToEnemy?.Invoke(hero);
                
                Destroy(gameObject);
            }
        }

        public void Initialize(float damage, Players player, InvokeApplyBuffToEnemy invokeApplyBuffToEnemy)
        {
            _damage = damage;
            _direction = player == Players.Player1 ? Vector2.right : Vector2.left;
            _invokeApplyBuffToEnemy = invokeApplyBuffToEnemy;
        }
    }
}

