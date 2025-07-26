using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

namespace DuelGame
{
    public class BuffReceiver : MonoBehaviour
    {
        [SerializeField] private BaseHero _hero;
        
        public delegate void BuffReceived(float duration, Sprite sp);
        public event BuffReceived OnBuffReceived;
        
        public CancellationTokenSource Cts {get; private set;}    
        
        private BuffsList _buffList;
        private Dictionary<BuffEnum, Func<Buff>> _buffsDictionary;

        private void Start()
        {
            _buffList = _hero.BuffList;
            _buffsDictionary = _hero.BuffsDictionary;
            Cts = new CancellationTokenSource();            
            
            /*_buffsDictionary = new Dictionary<BuffEnum, Func<Buff>>()
            {
                { BuffEnum.Poison, () => new PoisonBuff(Cts.Token) },
                { BuffEnum.Stun, () => new StunBuff() },
                { BuffEnum.DecreaseDamage, () => new DecreaseDamageBuff(Cts.Token) }
            };*/

            _hero.OnReceiveBuff += ReceiveBuffHandler;
        }

        private void OnDestroy()
        {
            _hero.OnReceiveBuff -= ReceiveBuffHandler;
            Cts.Cancel();
            Cts.Dispose();
            Cts = null;
        }

        private void ReceiveBuffHandler(BuffEnum buffEnum)
        {
            Buff currentBuff = _buffsDictionary[buffEnum]();

            
            currentBuff.Execute(_hero).Forget();
            
            OnBuffReceived?.Invoke(
                currentBuff.BuffDuration, 
                _buffList.ListOfEntities.First(x => x.BuffEnum == buffEnum).Sp.sprite);
        }
    }
}