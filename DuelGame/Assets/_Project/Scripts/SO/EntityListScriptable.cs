using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public abstract class EntityListScriptable<T> : ScriptableObject where T : class, INamedObject
    {
        public abstract List<T> ListOfEntities { get; set; }
    }
    public interface INamedObject { }
}