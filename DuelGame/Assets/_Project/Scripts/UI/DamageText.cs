using System;
using TMPro;
using UnityEngine;

namespace DuelGame
{
    public class DamageText : MonoBehaviour
    {
        public delegate void ReturnToPool(DamageText selfDamageText);
        
        private TextMeshProUGUI _damageText;

        private readonly float _defaultTimer = 2f;
        private readonly Color _defaultColor = new Color(1, 0.35f, 0.35f, 1);
        private readonly float _defaultAlphaInterval = 0.005f;
        private float _timer = 0;
        private float _alphaInterval = 0;

        private bool _isEnabled;

        private ReturnToPool _returnToPool;
        
        private void Awake()
        {
            _damageText = GetComponent<TextMeshProUGUI>();
            _timer = _defaultTimer;
        }
        
        private void OnEnable()
        {
            _isEnabled = true;
            _timer = _defaultTimer;
            _damageText.color = _defaultColor;
            _alphaInterval = _defaultAlphaInterval;
        }
        
        private void FixedUpdate()
        {
            if (!_isEnabled) 
                return;
            transform.position += Vector3.up * Time.fixedDeltaTime;

            _damageText.color = new Color(_damageText.color.r, _damageText.color.g, _damageText.color.b, _damageText.color.a - _alphaInterval);
            _alphaInterval += 0.0005f;

            _timer -= Time.fixedDeltaTime;
            if(_timer <= 0)
            {
                _returnToPool?.Invoke(this);
            }
        }

        public void Initialize(float damage, ReturnToPool returnToPool)
        {
            _damageText.text = (-damage).ToString();

            _returnToPool = returnToPool;
        }
        
        private void OnDisable()
        {
            _isEnabled = false;
        }
    }
}

