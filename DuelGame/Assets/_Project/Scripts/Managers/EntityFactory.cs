using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class EntityFactory : IDisposable
    {
        public bool IsSystemReady { get; private set; } = false;
        
        private readonly IRemoteConfigsLoader _remoteConfigsLoader;
        private readonly CancellationTokenSource _cts;
        
        private HeroesList _heroes; 
        private BuffsList _buffs;

        private Dictionary<BuffEnum, Func<Buff>> _buffsDictionary;
        
        public EntityFactory(IRemoteConfigsLoader remoteConfigsLoader)
        {
            _remoteConfigsLoader = remoteConfigsLoader;
            
            _cts = new CancellationTokenSource();
        }

        public void Init()
        {
            _remoteConfigsLoader.OnRemoteConfigsApplied += InitConfigs;
        }
        
        public void Dispose()
        {
            _cts.Cancel();
            _remoteConfigsLoader.OnRemoteConfigsApplied -= InitConfigs;
        }

        public async UniTask<BaseHero> SpawnRandomHero(Transform trans)
        {
            var entity = await _heroes.GetRandomHero(_cts.Token);
            var x = Object.Instantiate(entity.Hero, trans);
            x.Initialize(entity.HeroStats, _buffs, _buffsDictionary, entity.Aoc);
            return x;
        }

        public async UniTask<BaseHero> SpawnHeroByEnum(Transform trans, HeroEnum heroEnum)
        {
            var entity = await _heroes.GetHeroEntityByEnum(heroEnum, _cts.Token);
            var x = Object.Instantiate(entity.Hero, trans);
            x.Initialize(entity.HeroStats, _buffs, _buffsDictionary, entity.Aoc);
            return x;
        }        
        
        private void InitConfigs(GameLocalConfigs localConfigs, BuffsConfigs buffs)
        {
            _heroes = localConfigs.HeroesList;
            _buffs = localConfigs.BuffsList;
            
            _buffsDictionary = new Dictionary<BuffEnum, Func<Buff>>()
            {
                { BuffEnum.Poison, () => new PoisonBuff(buffs.Poison) },
                { BuffEnum.Stun, () => new StunBuff(buffs.Stun) },
                { BuffEnum.DecreaseDamage, () => new DecreaseDamageBuff(buffs.DecreaseDamage) }
            };

            IsSystemReady = true;
            Debug.Log($"Factory initialized");
        }
    }
}