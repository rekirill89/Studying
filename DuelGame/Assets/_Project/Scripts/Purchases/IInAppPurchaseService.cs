namespace DuelGame
{
    public interface IInAppPurchaseService
    {
        public bool IsSystemReady { get; }

        public void Init();
        public void BuyRemoveAds();

    }
}