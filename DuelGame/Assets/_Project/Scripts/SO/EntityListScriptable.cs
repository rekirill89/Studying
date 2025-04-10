using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuelGame
{
    public abstract class EntityListScriptable<T> : ScriptableObject where T : class, INamedObject
    {
        public abstract List<T> ListOfEntities { get; set; }

        public GameObject GetEntityObjByName(string name)
        {
            return ListOfEntities.First(x => x.Name == name).EntityObj;
        }
        
        public T GetEntityByName(string name)
        {
            return ListOfEntities.First(x => x.Name == name);
        }
        
        public T GetRandomEntity()
        {
            return ListOfEntities[Random.Range(0, ListOfEntities.Count)];
        }
    }
    public interface INamedObject
    {
        public string Name { get; set; }
        public GameObject EntityObj { get; set; }
    }
}

