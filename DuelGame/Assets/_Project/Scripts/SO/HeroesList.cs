using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
    public class HeroesList : EntityListScriptable<EntryHero>
    {
        [field:SerializeField] public override List<EntryHero> ListOfEntities { get; set; } = new List<EntryHero>();

        public async Task Init(ILocalAssetLoader assetLoader)
        {
            foreach (var hero in ListOfEntities)
            {
                var heroScript = await assetLoader.LoadAsset<GameObject>(hero.HeroScriptRef);
                
                hero.HeroStats = await assetLoader.LoadAsset<HeroStats>(hero.HeroStatsRef);
                hero.HeroScript = heroScript.GetComponent<BaseHero>();
            }
        }
        public EntryHero GetHeroEntityByEnum(HeroEnum heroEnum)
        {
            return ListOfEntities.First(x => x.HeroEnum == heroEnum);
        }
    }    
    [System.Serializable]
    public class EntryHero : INamedObject
    {
        public HeroStats HeroStats {get; set;}
        public BaseHero HeroScript {get; set;}
        public AssetReference HeroStatsRef;
        public AssetReference HeroScriptRef;
        public HeroEnum HeroEnum;
    }
}