using System;
using UnityEditor.Overlays;
using UnityEngine;

namespace DuelGame
{
    public class SaveService
    {
        public void SaveData<TData>(string key, TData data) where TData : class
        {
            string json = JsonUtility.ToJson(data);
            
            Debug.Log(json);
            PlayerPrefs.SetString(key, json);
        }

        public TData LoadData<TData>(string key) where TData : class
        {
            string json = PlayerPrefs.GetString(key);
            
            return JsonUtility.FromJson<TData>(json);
        }
    }
}