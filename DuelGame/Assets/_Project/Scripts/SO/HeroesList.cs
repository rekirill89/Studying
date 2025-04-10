using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
    public class HeroesList : EntityListScriptable<EntryHero>
    {
        [field:SerializeField] public override List<EntryHero> ListOfEntities { get; set; } = new List<EntryHero>();

        public HeroStats GetHeroStatsByName(string name)
        {
            return Instantiate(ListOfEntities.First(x => x.Name == name).HeroStats);
        }

        public EntryHero GetHeroEntityByEnum(HeroEnum heroEnum)
        {
            return ListOfEntities.First(x => x.HeroEnum == heroEnum);
        }
    }    
    [System.Serializable]
    public class EntryHero : INamedObject
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public GameObject EntityObj { get; set; }
        
        public HeroStats HeroStats;
        public BaseHero HeroScript;
        public HeroEnum HeroEnum;
    }

}
