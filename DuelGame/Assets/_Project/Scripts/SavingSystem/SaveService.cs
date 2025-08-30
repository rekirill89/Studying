using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace DuelGame
{
    public class SaveService
    {
        public event Action OnSkinSaveCleared;
        
        private const string USER_DATA_SAVE_KEY = "User data save";
        private const string SKINS_DATA_SAVE_KEY = "Skin data save";

        public void Save(UserData userData)
        {
            userData.SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string json = JsonUtility.ToJson(userData);
            string json = JsonConvert.SerializeObject(userData);
            
            PlayerPrefs.SetString(USER_DATA_SAVE_KEY, json);
        }

        public UserData Load() 
        {
            string json = PlayerPrefs.GetString(USER_DATA_SAVE_KEY);
            
            var userData = JsonConvert.DeserializeObject<UserData>(json);
            //var userData = JsonUtility.FromJson<UserData>(json);

            return userData;
        }
        
        public void SaveSkinsData(SkinsData skinsData)
        {
            skinsData.SaveTime = DateTime.Now.ToString(DateTimeConfig.DATE_TIME_FORMAT);
            //string json = JsonUtility.ToJson(skinsData);
            string json = JsonConvert.SerializeObject(skinsData);
            
            PlayerPrefs.SetString(SKINS_DATA_SAVE_KEY, json);
            Debug.Log("Successfully saved");
        }

        public SkinsData LoadSkinsData()
        {
            string json = PlayerPrefs.GetString(SKINS_DATA_SAVE_KEY);
            
            //var skinsData = JsonUtility.FromJson<SkinsData>(json);
            var skinsData = JsonConvert.DeserializeObject<SkinsData>(json);

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