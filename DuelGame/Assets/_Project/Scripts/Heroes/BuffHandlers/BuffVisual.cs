using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DuelGame;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BuffVisual : MonoBehaviour
    {
        [SerializeField] private BuffReceiver _buffReceiver;
        
        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _cts;

        private float _currenBuffDuration = 0;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _cts = _buffReceiver.Cts;
            
            _buffReceiver.OnBuffReceived += ShowBuffSprite;
        }

        private void OnDestroy()
        {
            _buffReceiver.OnBuffReceived -= ShowBuffSprite;
        }

        private void ShowBuffSprite(float duration, Sprite sprite)
        {
            ShowBuffSpriteUni(sprite, duration).Forget();
        }
        
        private async UniTask ShowBuffSpriteUni(Sprite sp, float duration)
        {
            _currenBuffDuration = duration;
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = sp;

            float step = 0.1f;
            while (_currenBuffDuration > 0 && !_cts.Token.IsCancellationRequested)
            {
                _currenBuffDuration -= step;
                await UniTask.Delay(TimeSpan.FromSeconds(step), cancellationToken: _cts.Token);
            }
            _spriteRenderer.enabled = false;
        }
    }
}

