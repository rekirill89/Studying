using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
public class HeroesList : ScriptableObject
{
    [System.Serializable]
    public class EntryHero
    {
        public string name;
        public GameObject heroObj;
        public HeroStats heroStats;
    }
    public List<EntryHero> heroes;

    public GameObject GetHeroByName(string name)
    {
        return heroes.First(x => x.name == name).heroObj;
    }
    public HeroStats GetHeroStatsByName(string name)
    {
        return Instantiate(heroes.First(x => x.name == name).heroStats);
    }
    public GameObject GetRandomHero()
    {
        int roll = Random.Range(0, heroes.Count);
        return heroes[roll].heroObj;
    }

}
