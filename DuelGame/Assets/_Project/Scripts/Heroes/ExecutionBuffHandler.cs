using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            _buffsEntities = _hero.buffList;

            _hero.OnApplyBuffToEnemy += ExecuteBuffToEnemy;
        }

        private void ExecuteBuffToEnemy(BaseHero hero)
        {
            Buff buff = _buffsDictionary[_chosenBuffEnum]();
            
            hero.currentBuffTask = buff.Execute(hero);
            hero.BuffAppliedInvoke(
                _buffsEntities.listOfEntities.First(
                    x => x.buffEnum == buff.buffEnum).sp.sprite, 
                buff.buffDuration);

        }

        private void OnDestroy()
        {
            _hero.OnApplyBuffToEnemy -= ExecuteBuffToEnemy;
            _cts.Cancel();
        }
    }
}

