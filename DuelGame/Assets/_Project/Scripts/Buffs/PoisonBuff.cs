using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class PoisonBuff : Buff
    {
        private readonly float _poisonDamagePerTick = 3.5f;
        private readonly float _startDelay = 0.3f;
        private readonly float _tickInterval = 0.3f;
        private const float POISON_BUFF_DURATION = 2.1f;
        private float _currenBuffDuration;

        private readonly CancellationToken _token;
        
        public PoisonBuff(CancellationToken token) : base(POISON_BUFF_DURATION)
        {
            _token = token;
            BuffEnum = BuffEnum.Poison;

            _currenBuffDuration = 0;
        }
        
        public override async UniTask Execute(BaseHero target, Sprite sprite)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_startDelay), cancellationToken: _token);
            
                await base.Execute(target, sprite);

                var intervalTimer = TimeSpan.FromSeconds(_tickInterval);
                while (_currenBuffDuration < BuffDuration && !_token.IsCancellationRequested)
                {
                    target.ChangeCurrentHealth(_poisonDamagePerTick);
                    _currenBuffDuration += _tickInterval;

                    await UniTask.Delay(intervalTimer, cancellationToken: _token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

