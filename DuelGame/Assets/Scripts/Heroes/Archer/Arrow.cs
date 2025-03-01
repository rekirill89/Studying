using UnityEngine;

namespace DuelGame
{
    public class Arrow : MonoBehaviour
    {
        private float _timerToDestroy = 5f;
        private float _damage = 0f;
        private float _speed = 15f;

        private Players _player;
        private BaseHero _hero;

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
            if (collision.TryGetComponent<BaseHero>(out BaseHero hero))
            {
                hero.TakeHit(_damage, BuffEnum.Poison);
                Debug.Log("Collision");
                Destroy(gameObject);
            }
        }

        public void Initialize(BaseHero hero, float damage, Players player)
        {
            _damage = damage;
            _player = player;
            _hero = hero;
        }
    }
}

