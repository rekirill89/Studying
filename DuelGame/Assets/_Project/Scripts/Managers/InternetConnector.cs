using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace DuelGame
{
    public class InternetConnector : IDisposable, IInternetConnector
    {
        public event Action OnConnected;
        
        public bool IsConnected { get; private set; } = false;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public void Dispose()
        {
            _cts.Cancel();
        }
        
        public async UniTask CheckInternetConnection()
        {
            using (var request = UnityWebRequest.Get("https://www.google.com"))
            {
                request.timeout = 1;

                try
                {
                    await request.SendWebRequest();
                
                    Debug.Log("Connection successful");
                }
                catch
                {
                    Debug.LogWarning("Connection failed");
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    IsConnected = true;
                    OnConnected?.Invoke();
                }
                else
                    IsConnected = false;
            }
        }

        public async UniTask MonitorInternetConnection()
        {
            while (!_cts.IsCancellationRequested)
            {
                await CheckInternetConnection();
                
                await UniTask.Delay(3000, cancellationToken: _cts.Token);
            }
        }
    }
}