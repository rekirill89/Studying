using System;
using System.Collections.Generic;
//using UnityEditor.Overlays;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        private readonly Dictionary<SaveType, string> _saveKeys = new Dictionary<SaveType, string>()
        {
            { SaveType.Auto, "Auto save" },
            { SaveType.Manual, "Manual save" },
        };
        
        public void SaveData(BattleData data, SaveType saveType)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(_saveKeys[saveType], json);
        }
        
        public BattleData LoadData(SaveType saveType) 
        {
            string json = PlayerPrefs.GetString(_saveKeys[saveType]);
            
            return JsonUtility.FromJson<BattleData>(json);
        }
    }

    public enum SaveType
    {
        Auto,
        Manual
    }
}