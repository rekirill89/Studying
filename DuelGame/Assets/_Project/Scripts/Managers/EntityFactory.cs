using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class EntityFactory : IDisposable/*, IInitializable*/
    {
        public bool IsSystemReady { get; private set; } = false;
        
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly IRemoteConfigsManager _remoteConfigsManager;
        private readonly CancellationTokenSource _cts;
        
        private HeroesList _heroes; 
        private BuffsList _buffs;

        private Dictionary<BuffEnum, Func<Buff>> _buffsDictionary;
        
        public EntityFactory(GlobalAssetsLoader globalAssetsLoader, IRemoteConfigsManager remoteConfigsManager)
        {
            _globalAssetsLoader = globalAssetsLoader;
            _remoteConfigsManager = remoteConfigsManager;
            
            _cts = new CancellationTokenSource();
        }

        public void Init()
        {
            _remoteConfigsManager.OnRemoteConfigsApplied += InitConfigs;
        }
        
        public void Dispose()
        {
            _cts.Cancel();
            _remoteConfigsManager.OnRemoteConfigsApplied -= InitConfigs;
        }

        public async UniTask<BaseHero> SpawnRandomHero(Transform trans)
        {
            var entity = await _heroes.GetRandomHero(_cts.Token);
            var x = Object.Instantiate(entity.Hero, trans);
            x.Initialize(entity.HeroStats, _buffs, _buffsDictionary);
            return x;
        }

        public async UniTask<BaseHero> SpawnHeroByEnum(Transform trans, HeroEnum heroEnum)
        {
            var entity = await _heroes.GetHeroEntityByEnum(heroEnum, _cts.Token);
            var x = Object.Instantiate(entity.Hero, trans);
            x.Initialize(entity.HeroStats, _buffs, _buffsDictionary);
            return x;
        }        
        
        private void InitConfigs(GameLocalConfigs localConfigs, BuffsRemoteConfigs buffs)
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