using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Zenject;

namespace DuelGame
{
    public class UnityCloudSaveService
    {
        private const string USER_DATA_KEY = "UserData";

        private readonly InternetConnector _internetConnector;
        
        private readonly HashSet<string> _saveKeys = new HashSet<string>()
        {
            USER_DATA_KEY,
        };

        public UnityCloudSaveService(InternetConnector internetConnector)
        {
            _internetConnector = internetConnector;
        }
        
        public async UniTask SaveUserDataAsync(UserData userData)
        {
            if(!_internetConnector.IsConnected)
                return;
            
            userData.SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string json = JsonUtility.ToJson(userData);

            var saveData = new Dictionary<string, object>
            {
                { USER_DATA_KEY, json }
            };
            
            await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
            Debug.Log("Data saved in cloud");
        }

        public async UniTask<UserData> LoadUserDataAsync()
        {
            if (!_internetConnector.IsConnected)
                Debug.LogError("Cannot load cloud save, no internet!");
            
            var x = await CloudSaveService.Instance.Data.Player.LoadAsync(_saveKeys);
            var userDataItem = x[USER_DATA_KEY];

            var jToken = JToken.FromObject(userDataItem.Value);
            Debug.Log(jToken.Type);
            string json = jToken.Type == JTokenType.String ? jToken.ToString() : jToken["Value"]?.ToString();
            
            Debug.Log("Data loaded from cloud");
            var userData = JsonUtility.FromJson<UserData>(json);
            return userData;
        }
    }
}