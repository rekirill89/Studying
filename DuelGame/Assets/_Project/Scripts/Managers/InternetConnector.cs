using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace DuelGame
{
    public class InternetConnector
    {
        public bool IsConnected { get; private set; } = false;

        public async UniTask CheckInternetConnection()
        {
            using (var request = UnityWebRequest.Get("https://www.google.com"))
            {
                request.timeout = 3;

                try
                {
                    await request.SendWebRequest();
                    
                    Debug.Log("Connection successful");
                }
                catch
                {
                    Debug.LogWarning("Shit, connection failed");
                }

                if (request.result == UnityWebRequest.Result.Success)
                    IsConnected = true;
                else
                    IsConnected = false;
            }
        }
    }
}