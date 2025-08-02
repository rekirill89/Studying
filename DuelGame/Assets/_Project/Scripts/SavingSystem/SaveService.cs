using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        private const string SAVE_KEY = "Auto save";

        public void Save(UserData userData)
        {
            Debug.Log(userData.BattleData.Player1 + " " + userData.BattleData.Player2);
            string json = JsonUtility.ToJson(userData);
            
            PlayerPrefs.SetString(SAVE_KEY, json);
        }
        
        public UserData Load() 
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            
            var userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log(userData.BattleData.Player1 + " " + userData.BattleData.Player2);

            return userData;
        }
    }
}