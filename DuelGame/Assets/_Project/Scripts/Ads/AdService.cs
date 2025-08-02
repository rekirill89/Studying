using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.LevelPlay;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class AdService : IInitializable, IDisposable, IAdService
    {
        public event Action OnRewardedAdClosed;
        public event Action OnInterstiateAdClosed;
        
        private BattleManager _battleManager;
        
        private LevelPlayRewardedAd _levelPlayRewardedAd;
        private LevelPlayInterstitialAd _levelPlayInterstitialAd;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public void Initialize()
        {
            LevelPlay.OnInitFailed += LevelPlayOnInitFailedHandler;
            LevelPlay.OnInitSuccess += LevelPlayOnInitSuccessHandler;
            
            LevelPlay.Init(AdConfig.APP_KEY);
        }
        
        public void Dispose()
        {
            _cts.Cancel();
            
            _levelPlayRewardedAd.OnAdLoadFailed -= RewardedAdOnLoadFailedHandler;
            _levelPlayRewardedAd.OnAdClosed -= RewardedAdOnClosedHandler;
            
            _levelPlayInterstitialAd.OnAdLoadFailed -= InterstitialAdOnLoadFailedHandler;
            _levelPlayInterstitialAd.OnAdClosed -= InterstitialAdOnClosedHandler;
        }

        public void ShowRewardedAd()
        {
            if (_levelPlayRewardedAd.IsAdReady())
            {
                _levelPlayRewardedAd.ShowAd();
            }
        }

        public void ShowInterstitialAd()
        {
            if (_levelPlayInterstitialAd.IsAdReady())
            {
                _levelPlayInterstitialAd.ShowAd();
            }
        }

        private void LevelPlayOnInitFailedHandler(LevelPlayInitError obj)
        {
            Debug.Log("LevelPlayOnInitFailedHandler");
        }

        private void LevelPlayOnInitSuccessHandler(LevelPlayConfiguration obj)
        {
            Debug.Log("LevelPlayOnInitSuccessHandler");
            EnableAds();
        }

        private void EnableAds()
        {
            _levelPlayRewardedAd = new LevelPlayRewardedAd(AdConfig.AD_REWARDED_VIDEO);
            _levelPlayInterstitialAd = new LevelPlayInterstitialAd(AdConfig.AD_INTERSTITIAL_VIDEO);
            
            _levelPlayRewardedAd.OnAdLoadFailed += RewardedAdOnLoadFailedHandler;
            _levelPlayRewardedAd.OnAdClosed += RewardedAdOnClosedHandler;
            
            _levelPlayInterstitialAd.OnAdLoadFailed += InterstitialAdOnLoadFailedHandler;
            _levelPlayInterstitialAd.OnAdClosed += InterstitialAdOnClosedHandler;
            
            _levelPlayInterstitialAd.LoadAd();
            _levelPlayRewardedAd.LoadAd();
        }

        private void InterstitialAdOnLoadFailedHandler(LevelPlayAdError error)
        {
            Debug.Log(error.ErrorMessage);
            try
            {
                ReloadInterstitialAd().Forget();        
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }

        private void RewardedAdOnLoadFailedHandler(LevelPlayAdError error)
        {
            Debug.Log(error.ErrorMessage);
            ReloadRewardedAd().Forget();
        }

        private void InterstitialAdOnClosedHandler(LevelPlayAdInfo _)
        {
            OnInterstiateAdClosed?.Invoke();
            _levelPlayInterstitialAd.LoadAd();
        }

        private void RewardedAdOnClosedHandler(LevelPlayAdInfo _)
        {
            OnRewardedAdClosed?.Invoke();
            _levelPlayRewardedAd.LoadAd();
        }

        private async UniTask ReloadRewardedAd()
        {
            try
            {
                Debug.Log("Reloading Rewarded Ad");
                await UniTask.Yield(cancellationToken: _cts.Token);
                _levelPlayRewardedAd.LoadAd();
            }
            catch (OperationCanceledException )
            {
                Debug.LogError("Loading ads cancelled");
                throw;
            }
        }

        private async UniTask ReloadInterstitialAd()
        {
            try
            {
                Debug.Log("Reloading Interstatial Ad");
                await UniTask.Yield(cancellationToken: _cts.Token);
                _levelPlayInterstitialAd.LoadAd();
            }
            catch (OperationCanceledException )
            {
                Debug.LogError("Loading ads cancelled");
                throw;
            }
        }
    }
}