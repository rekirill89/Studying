using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "BuffsList", menuName = "Scriptable Objects/BuffsList")]
    public class BuffsList : EntityListScriptable<EntryBuff>
    {
        [field:SerializeField] public override List<EntryBuff> listOfEntities { get; set; } = new List<EntryBuff>();
    }        

    [System.Serializable]
     public class EntryBuff : INamedObject
     {
        [field: SerializeField] public string name { get; set; }
        [field: SerializeField] public GameObject entityObj { get; set; }
        public SpriteRenderer sp;
        //public Buff buffScript;
        public BuffEnum buff;
     }

    public enum BuffEnum
    {
        Poison,
        DecreaseDamage,
        Stun,
        None
    }
}

