using System;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        public delegate void ArrowHit(Arrow arrow);
        public event ArrowHit OnArrowHit;
        
        private const int BODY_LAYER = 0;
        private readonly float _speed = 15f;
        private float _timerToDestroy = 5f;

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
            if (collision.TryGetComponent<BaseHero>(out BaseHero hero) && collision.layerOverridePriority == BODY_LAYER)
            {
                OnArrowHit?.Invoke(this);
                
                Destroy(gameObject);
            }
        }

        public void Initialize(Players player)
        {
            _direction = player == Players.Player1 ? Vector2.right : Vector2.left;
        }
    }
}