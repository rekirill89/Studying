using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    public class BuffExecutor : MonoBehaviour
    {
        [SerializeField] private BaseHero _hero;
        [SerializeField] private BuffEnum _chosenBuffEnum;
        
        private void Start()
        {
            _hero.OnApplyBuffToEnemy += ExecuteBuffToEnemy;
        }
        
        private void OnDestroy()
        {
            _hero.OnApplyBuffToEnemy -= ExecuteBuffToEnemy;
        }
        
        private void ExecuteBuffToEnemy(BaseHero enemy)
        {
            enemy.BuffReceivedInvoke(_chosenBuffEnum);
        }
    }
}