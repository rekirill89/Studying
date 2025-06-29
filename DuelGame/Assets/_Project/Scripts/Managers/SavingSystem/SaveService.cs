using System;
using System.Collections.Generic;
//using UnityEditor.Overlays;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        //private static string SAVE_AUTO = "Auto save";
        //private static string SAVE_MANUAL = "Manual save";

        private Dictionary<SaveType, string> _saveKeys = new Dictionary<SaveType, string>()
        {
            { SaveType.Auto, "Auto save" },
            { SaveType.Manual, "Manual save" },
        };
        
        /*public void SaveAutoData(BattleData data)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(SAVE_AUTO, json);
        }*/
        
        public void SaveData(BattleData data, SaveType saveType)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(_saveKeys[saveType], json);
        }
        
        /*public void SaveManualData(BattleData data)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(SAVE_MANUAL, json);
        }*/

        public BattleData LoadData(SaveType saveType) 
        {
            string json = PlayerPrefs.GetString(_saveKeys[saveType]);
            
            return JsonUtility.FromJson<BattleData>(json);
        }
        
        /*public BattleData LoadAutoData() 
        {
            string json = PlayerPrefs.GetString(SAVE_AUTO);
            
            return JsonUtility.FromJson<BattleData>(json);
        }
        
        public BattleData LoadManualData() 
        {
            string json = PlayerPrefs.GetString(SAVE_MANUAL);
            
            return JsonUtility.FromJson<BattleData>(json);
        }*/
    }

    public enum SaveType
    {
        Auto,
        Manual
    }
}