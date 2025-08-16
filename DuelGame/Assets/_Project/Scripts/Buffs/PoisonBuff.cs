using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class PoisonBuff : Buff
    {
        public override float BuffDuration { get; protected set; } = 2.1f;
        
        private readonly float _poisonDamagePerTick = 3.5f;
        private readonly float _startDelay = 0.3f;
        private readonly float _tickInterval = 0.3f;
        
        private float _currenBuffDuration;
        
        public PoisonBuff(PoisonBuffConfig config = null)
        {
            BuffEnum = BuffEnum.Poison;
            _currenBuffDuration = 0;
            
            if(config == null)
                return;
            
            BuffDuration = config.Duration;
            _poisonDamagePerTick = config.DamagePerTick;
            _startDelay = config.StartDelay;
            _tickInterval = config.TickInterval;
        }
        
        public override async UniTask Execute(BaseHero target)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_startDelay), cancellationToken: Token);
        
            await base.Execute(target);

            var intervalTimer = TimeSpan.FromSeconds(_tickInterval);
            while (_currenBuffDuration < BuffDuration && !Token.IsCancellationRequested)
            {
                target.TakeHit(_poisonDamagePerTick, false);
                _currenBuffDuration += _tickInterval;

                await UniTask.Delay(intervalTimer, cancellationToken: Token);
            }
        }
    }
}