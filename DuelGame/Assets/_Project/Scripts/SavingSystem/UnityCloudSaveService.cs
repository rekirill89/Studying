using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Core;
using Zenject;

namespace DuelGame
{
    public class UnityCloudSaveService
    {
        private const string USER_DATA_KEY = "UserData";
        
        private readonly IInternetConnector _internetConnector;
        
        private readonly HashSet<string> _saveKeys = new HashSet<string>()
        {
            USER_DATA_KEY,
        };

        public UnityCloudSaveService(IInternetConnector internetConnector)
        {
            _internetConnector = internetConnector;
        }
        
        public async UniTask SaveUserDataAsync(UserData userData)
        {
            if(!_internetConnector.IsConnected)
                return;
            
            userData.SaveTime = DateTime.Now.ToString(DateTimeConfig.DATE_TIME_FORMAT);
            string json = JsonConvert.SerializeObject(userData);
            //string json = JsonUtility.ToJson(userData);

            var saveData = new Dictionary<string, object>
            {
                { USER_DATA_KEY, json }
            };

            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
                
                Debug.Log("Data saved in cloud");
            }
            catch (RequestFailedException)
            {
                Debug.LogWarning("Failed to save data in cloud, no internet. Try again later");
                //throw;
            }
        }

        public async UniTask<UserData> LoadUserDataAsync()
        {
            if (!_internetConnector.IsConnected)
                Debug.LogError("Cannot load cloud save, no internet!");

            Dictionary<string, Item> x = new Dictionary<string, Item>();
            try
            {
                x = await CloudSaveService.Instance.Data.Player.LoadAsync(_saveKeys);
            }
            catch (RequestFailedException)
            {
                Debug.LogWarning("Failed to load cloud save, no internet. Try again later");
                throw;
            }
            
            var userDataItem = x[USER_DATA_KEY];

            var jToken = JToken.FromObject(userDataItem.Value);
            string json = jToken.Type == JTokenType.String ? jToken.ToString() : jToken["Value"]?.ToString();
            
            Debug.Log("Data loaded from cloud");
            var userData = JsonConvert.DeserializeObject<UserData>(json);
            return userData;
        }
    }
}