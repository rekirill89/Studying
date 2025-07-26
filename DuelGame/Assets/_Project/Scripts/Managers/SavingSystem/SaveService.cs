using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        private const string SAVE_KEY = "Auto save";
        /*private const string MANUAL_SAVE_KEY = "Manual save";*/

        //public void ManualSave(BattleData data) => Save(data, MANUAL_SAVE_KEY);
        //public void AutoSave(BattleData data) => Save(data, SAVE_KEY);
        
        /*public BattleData ManualLoad() => Load(MANUAL_SAVE_KEY);
        public BattleData AutoLoad() => Load(SAVE_KEY);*/
        
        public void Save(BattleData data/*, string key*/)
        {
            string json = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(SAVE_KEY, json);
        }
        
        public BattleData Load(/*string key*/) 
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            
            return JsonUtility.FromJson<BattleData>(json);
        }
    }
}