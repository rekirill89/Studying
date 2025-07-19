using System;
using Firebase;
using Firebase.Analytics;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class FireBaseInit
    {
        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                if (task.Result == DependencyStatus.Available) {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    Debug.Log("Firebase ready!");
                } else {
                    Debug.LogError("Error Firebase: " + task.Result);
                }
            });
        }
    }
}

