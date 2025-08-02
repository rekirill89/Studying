using System;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class PurchasesDataController : IInitializable
    {
        private readonly DataCache _dataCache;
        private readonly SaveService _saveService;

        private readonly bool _isCheckPurchases = true;
        
        public PurchasesDataController(
            DataCache dataCache, 
            SaveService saveService)
        {
            _dataCache = dataCache;
            _saveService = saveService;
        }
        
        public void Initialize()
        {
            CheckPurchases();
        }

        public void SaveRemoveAdsPurchase()
        {
            Debug.Log("Done!");
            var x = _saveService.Load();
            x.IsAdsRemoved = true;
            _saveService.Save(x);
            
            _dataCache.RemoveAds();
        }

        private void CheckPurchases()
        {
            if(_isCheckPurchases)
                return;
            if (_saveService.Load().IsAdsRemoved)
            {
                Debug.Log("Purchases true");
                _dataCache.RemoveAds();
            }
            Debug.Log("Purchases false");
        }
    }
}