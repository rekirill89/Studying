using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
    public class HeroesList : EntityListScriptable<EntryHero>
    {
        [field:SerializeField] public override List<EntryHero> ListOfEntities { get; set; } = new List<EntryHero>();

        private ILocalAssetLoader _localAssetLoader;

        private void OnDestroy()
        {
            foreach (var hero in ListOfEntities)
            {
                _localAssetLoader.UnloadAsset(hero.HeroStatsRef);
                _localAssetLoader.UnloadAsset(hero.HeroRef);
            }
        }

        public void Init(ILocalAssetLoader assetLoader)
        {
            _localAssetLoader = assetLoader;
        }
        
        public async UniTask<EntryHero> GetHeroEntityByEnum(HeroEnum heroEnum, CancellationToken token)
        {
            var hero = ListOfEntities.First(x => x.HeroEnum == heroEnum);
            if(!hero.IsLoaded)
                await LoadHero(hero, token);
            
            return hero;
        }
        
        public async UniTask<EntryHero> GetRandomHero(CancellationToken token)
        {
            var hero = ListOfEntities[Random.Range(0, ListOfEntities.Count)];
            if (!hero.IsLoaded)
                await LoadHero(hero, token);
            
            return hero;
        }

        public async UniTask LoadAllHeroes(CancellationToken token)
        {
            foreach (var hero in ListOfEntities)
            {
                if(!hero.IsLoaded)
                    await LoadHero(hero, token);
            }
            Debug.Log("LoadAllHeroes success");
        }
        
        private async UniTask LoadHero(EntryHero hero, CancellationToken token)
        {
            hero.Hero = await _localAssetLoader.LoadHero(hero.HeroRef, token);
            hero.HeroStats = await _localAssetLoader.LoadHeroStats(hero.HeroStatsRef, token);
            hero.IsLoaded = true;
            
            Debug.Log("LoadHero success");
        }
    }    
    [System.Serializable]
    public class EntryHero : INamedObject
    {
        public HeroStats HeroStats {get; set;}
        public BaseHero Hero {get; set;}
        public bool IsLoaded {get; set;} = false;
        public AssetReference HeroStatsRef;
        public AssetReference HeroRef;
        public HeroEnum HeroEnum;
    }
}