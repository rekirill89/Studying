using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffsList", menuName = "Scriptable Objects/BuffsList")]
public class BuffsList : ScriptableObject
{
    [System.Serializable]
    public class EntryBuff
    {
        public string name;
        public GameObject buffObj;
    }
    public List<EntryBuff> listOfBuffs = new List<EntryBuff>(); 
    public GameObject GetBuffObjByName(string name)
    {
        return listOfBuffs.First(x => x.name == name).buffObj;
    } 
    
}
