using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuelGame
{
    public abstract class EntityListScriptable<T> : ScriptableObject where T : class, INamedObject
    {
        public abstract List<T> ListOfEntities { get; set; }
        
        public T GetRandomEntity()
        {
            return ListOfEntities[Random.Range(0, ListOfEntities.Count)];
        }
    }
    public interface INamedObject
    {

    }
}