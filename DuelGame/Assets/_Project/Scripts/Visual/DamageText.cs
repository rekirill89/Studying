using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DuelGame
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 1.5f;
        [SerializeField] private float _moveYTarget = 1.5f;
        [SerializeField] private float _alphaTarget = 0f;
        
        [SerializeField] private Color _defaultColor = new Color(1, 0.35f, 0.35f, 1);
        [SerializeField] private Vector2 _startPosition = new Vector2(0, 0);
        
        public event Action<DamageText> OnComplete; 
        
        private TextMeshProUGUI _damageText;
        
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
            //OnComplete.RemoveAllListeners();
        }
        
        public void Initialize(float damage)
        {            
            _damageText.transform.position = _startPosition;
            _damageText.text = (-damage).ToString();
            _damageText.color = _defaultColor;
            
            _moveTween = transform
                .DOMoveY(transform.position.y + _moveYTarget, _lifeTime)
                .SetEase(Ease.Linear);
            
            _fadeTween = _damageText
                .DOFade(_alphaTarget, _lifeTime)
                .OnComplete(() => OnComplete?.Invoke(this));
        }
    }
}