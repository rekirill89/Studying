using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class AuthService 
    {
        public bool IsSystemReady { get; private set; } = false;

        private readonly InternetConnector _internetConnector;
        
        public AuthService(InternetConnector internetConnector)
        {
            _internetConnector = internetConnector;    
        }
        
        public void Init()
        {
            SignIn();
        }

        private void SignIn()
        {
            if (!_internetConnector.IsConnected)
            {
                Debug.LogWarning("Sign in failed, no internet!");
                IsSystemReady = true;

                return;
            }
            SignInAnonymous().Forget();
        }

        private async UniTask SignInAnonymous()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("SignIn Anonymous");
                IsSystemReady = true;
            }
            catch (AuthenticationException e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}