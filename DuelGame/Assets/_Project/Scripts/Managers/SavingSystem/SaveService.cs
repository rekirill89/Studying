using System;
using System.Collections.Generic;
//using UnityEditor.Overlays;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        private const string AUTO_SAVE_KEY = "Auto save";
        private const string MANUAL_SAVE_KEY = "Manual save";

        public void ManualSave(BattleData data) => Save(data, MANUAL_SAVE_KEY);
        public void AutoSave(BattleData data) => Save(data, AUTO_SAVE_KEY);
        
        public BattleData ManualLoad() => Load(MANUAL_SAVE_KEY);
        public BattleData AutoLoad() => Load(AUTO_SAVE_KEY);
        
        private void Save(BattleData data, string key)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(key, json);
        }
        
        private BattleData Load(string key) 
        {
            string json = PlayerPrefs.GetString(key);
            
            return JsonUtility.FromJson<BattleData>(json);
        }
    }
}