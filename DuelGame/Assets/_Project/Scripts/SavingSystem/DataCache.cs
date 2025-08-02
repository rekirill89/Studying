using Unity.VisualScripting;

namespace DuelGame
{
    public class DataCache
    {
        public bool IsAdsRemoved { get; private set; } = false;

        private BattleData _battleData = null;

        private bool _isLoadingDataAvailable = false;
        
        public void RemoveAds() => IsAdsRemoved = true;
        
        public BattleData ConsumeBattleData()
        {
            if (_isLoadingDataAvailable)
            {
                _isLoadingDataAvailable = false;
                return _battleData;
            } 
            return null;
        }

        public void SetBattleData(BattleData battleData)
        {
            _battleData = battleData;
        }

        public void ChangeLoadingStatus(bool isLoading)
        {
            _isLoadingDataAvailable = isLoading;
        }
    }
}