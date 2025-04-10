using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    public class ExecutionBuffHandler : MonoBehaviour
    {
        [SerializeField] private BaseHero _hero;
        [SerializeField] private BuffEnum _chosenBuffEnum;
        
        private CancellationTokenSource _cts;
        private Dictionary<BuffEnum, Func<Buff>> _buffsDictionary;

        private BuffsList _buffsEntities;

        private void Awake()
        {
            _cts = new CancellationTokenSource();
            
            _buffsDictionary = new Dictionary<BuffEnum, Func<Buff>>()
            {
                { BuffEnum.Poison, () => new PoisonBuff(_cts.Token) },
                { BuffEnum.Stun, () => new StunBuff() },
                { BuffEnum.DecreaseDamage, () => new DecreaseDamageBuff(_cts.Token) }
            };
        }

        private void Start()
        {
            _buffsEntities = _hero.BuffList;

            _hero.OnApplyBuffToEnemy += ExecuteBuffToEnemy;
            _hero.OnPlayerStop += StopBuffTask;
        }
        
        private void OnDestroy()
        {
            _hero.OnPlayerStop -= StopBuffTask;
            _hero.OnApplyBuffToEnemy -= ExecuteBuffToEnemy;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void StopBuffTask()
        {
            _cts.Cancel();
        }

        private void ExecuteBuffToEnemy(BaseHero hero)
        {
            Buff buff = _buffsDictionary[_chosenBuffEnum]();
            
            buff.Execute(hero, _buffsEntities.ListOfEntities.First(x => x.BuffEnum == buff.BuffEnum).Sp.sprite).Forget();
        }
    }
}

