using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "BuffsList", menuName = "Scriptable Objects/BuffsList")]
    public class BuffsList : EntityListScriptable<EntryBuff>
    {
        [field:SerializeField] public override List<EntryBuff> ListOfEntities { get; set; } = new List<EntryBuff>();
    }        

    [System.Serializable]
     public class EntryBuff : INamedObject
     {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public GameObject EntityObj { get; set; }
        public SpriteRenderer Sp;
        public BuffEnum BuffEnum;
     }

    public enum BuffEnum
    {
        Poison,
        DecreaseDamage,
        Stun,
        None
    }
}

