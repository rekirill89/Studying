using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
    public class HeroesList : EntityListScriptable<EntryHero>
    {
        [field:SerializeField] public override List<EntryHero> ListOfEntities { get; set; } = new List<EntryHero>();

        private ILocalAssetLoader _localAssetLoader;
        
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
        
        private async UniTask LoadHero(EntryHero hero, CancellationToken token)
        {
            hero.HeroScript = await _localAssetLoader.LoadHeroScript(hero.HeroScriptRef, token);
            hero.HeroStats = await _localAssetLoader.LoadHeroStats(hero.HeroStatsRef, token);
            hero.IsLoaded = true;
        }
    }    
    [System.Serializable]
    public class EntryHero : INamedObject
    {
        public HeroStats HeroStats {get; set;}
        public BaseHero HeroScript {get; set;}
        public bool IsLoaded {get; set;} = false;
        public AssetReference HeroStatsRef;
        public AssetReference HeroScriptRef;
        public HeroEnum HeroEnum;
    }
}