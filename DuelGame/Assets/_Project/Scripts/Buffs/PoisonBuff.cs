using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class PoisonBuff : Buff
    {
        private readonly float _poisonPeriodicalDamage = 3.5f;
        private readonly float _interval = 0.3f;

        private float _currenBuffDuration;

        private readonly CancellationToken _token;
        
        public PoisonBuff(CancellationToken token) : base(2.1f)
        {
            _token = token;
            buffEnum = BuffEnum.Poison;

            _currenBuffDuration = 0;
        }
        
        public override async UniTask Execute(BaseHero target)
        {
            Debug.Log("Poison buff");
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _token);

            var intervalTimer = TimeSpan.FromSeconds(_interval);
            while (_currenBuffDuration < buffDuration && !_token.IsCancellationRequested)
            {
                target.ChangeCurrentHealth(_poisonPeriodicalDamage);
                _currenBuffDuration += _interval;

                await UniTask.Delay(intervalTimer, cancellationToken: _token);
            }
        }
    }
}

