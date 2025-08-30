using System;
using Firebase;
using Firebase.Analytics;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class FireBaseInit
    {
        public bool IsSystemReady { get; private set; } = false;
        
        public void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                if (task.Result == DependencyStatus.Available) {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    Debug.Log("Firebase ready!");
                    //OnInitSuccess?.Invoke();
                } else {
                    Debug.LogError("Error Firebase: " + task.Result);
                }
            });
            Debug.Log($"{this} is ready");
            IsSystemReady = true;
        }
    }
}

