using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
    public class HeroesList : EntityListScriptable<EntryHero>
    {
        [field:SerializeField] public override List<EntryHero> listOfEntities { get; set; } = new List<EntryHero>();

        public HeroStats GetHeroStatsByName(string name)
        {
            return Instantiate(listOfEntities.First(x => x.name == name).heroStats);
        }

        public EntryHero GetHeroEntityByEnum(HeroEnum heroEnum)
        {
            return listOfEntities.First(x => x.heroEnum == heroEnum);
        }
    }    
    [System.Serializable]
    public class EntryHero : INamedObject
    {
        [field: SerializeField] public string name { get; set; }
        [field: SerializeField] public GameObject entityObj { get; set; }
        
        public HeroStats heroStats;
        public BaseHero heroScript;
        public HeroEnum heroEnum;
    }

}
