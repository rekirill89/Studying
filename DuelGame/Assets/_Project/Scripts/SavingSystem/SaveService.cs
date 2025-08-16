using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        public event Action OnSkinSaveCleared;
        
        private const string USER_DATA_SAVE_KEY = "User data save";
        private const string SKINS_DATA_SAVE_KEY = "Skin data save";

        public void Save(UserData userData)
        {
            string json = JsonUtility.ToJson(userData);
            
            PlayerPrefs.SetString(USER_DATA_SAVE_KEY, json);
        }

        public UserData Load() 
        {
            string json = PlayerPrefs.GetString(USER_DATA_SAVE_KEY);
            
            var userData = JsonUtility.FromJson<UserData>(json);

            return userData;
        }
        
        public void SaveSkinsData(SkinsData skinsData)
        {
            string json = JsonUtility.ToJson(skinsData);
            
            PlayerPrefs.SetString(SKINS_DATA_SAVE_KEY, json);
            Debug.Log("Successfully saved");
        }

        public SkinsData LoadSkinsData()
        {
            string json = PlayerPrefs.GetString(SKINS_DATA_SAVE_KEY);
            
            var skinsData = JsonUtility.FromJson<SkinsData>(json);

            return skinsData;
        }

        public void ClearSkinsData()
        {
            PlayerPrefs.DeleteKey(SKINS_DATA_SAVE_KEY);
            PlayerPrefs.Save();
            OnSkinSaveCleared?.Invoke();
        }
    }
}