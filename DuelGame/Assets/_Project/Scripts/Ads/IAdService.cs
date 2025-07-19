using System;

namespace DuelGame
{
    public interface IAdService
    {
        public event Action OnRewardedAdClosed;
        public event Action OnInterstiateAdClosed;

        public void ShowRewardedAd();
        public void ShowInterstitialAd();

    }
}