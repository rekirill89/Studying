using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuelGame
{
    public abstract class EntityListScriptable<T> : ScriptableObject where T : class, INamedObject
    {
        public abstract List<T> listOfEntities { get; set; }

        public GameObject GetEntityObjByName(string name)
        {
            return listOfEntities.First(x => x.name == name).entityObj;
        }
        
        public T GetEntityByName(string name)
        {
            return listOfEntities.First(x => x.name == name);
        }
        
        public T GetRandomEntity()
        {
            return listOfEntities[Random.Range(0, listOfEntities.Count)];
        }
    }
    public interface INamedObject
    {
        public string name { get; set; }
        public GameObject entityObj { get; set; }
    }
}

