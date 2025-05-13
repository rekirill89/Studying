using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace DuelGame
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DamageText : MonoBehaviour
    {
        public delegate void ReturnToPool(DamageText selfDamageText);
        private ReturnToPool _returnToPool;
                
        private TextMeshProUGUI _damageText;
        
        private readonly float _lifeTime = 1.5f;
        private readonly float _moveYTarget = 1.5f;
        private readonly float _alphaTarget = 0f;
        
        private readonly Color _defaultColor = new Color(1, 0.35f, 0.35f, 1);
        private readonly Vector2 _startPosition = new Vector2(0, 0);

        private Tween _moveTween;
        private Tween _fadeTween;
        
        private void Awake()
        {
            _damageText = GetComponent<TextMeshProUGUI>();
        }
        
        private void OnDisable()
        {
            _moveTween?.Kill();
            _fadeTween?.Kill();
        }
        
        public void Initialize(float damage, ReturnToPool returnToPool)
        {            
            _returnToPool = returnToPool;
            
            _damageText.transform.position = _startPosition;
            _damageText.text = (-damage).ToString();
            _damageText.color = _defaultColor;
            
            _moveTween = transform.DOMoveY(transform.position.y + _moveYTarget, _lifeTime).SetEase(Ease.Linear);
            
            _fadeTween = _damageText.DOFade(_alphaTarget, _lifeTime).OnComplete(() => _returnToPool?.Invoke(this));
        }
    }
}

