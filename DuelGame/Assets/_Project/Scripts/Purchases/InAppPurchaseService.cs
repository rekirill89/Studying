using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Zenject;

namespace DuelGame
{
    public class InAppPurchaseService :  IDetailedStoreListener, IInAppPurchaseService
    {
        public bool IsSystemReady { get; private set; } = false;

        private const string REMOVE_ADS_PRODUCT = "remove_ads";
        private readonly PurchasesDataController _purchasesDataController;
        private readonly InternetConnector _internetConnector;

        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;
        private bool _isSystemReady;

        public InAppPurchaseService(PurchasesDataController purchasesDataController, InternetConnector internetConnector)
        {
            _purchasesDataController = purchasesDataController;
            _internetConnector = internetConnector;
        }

        public void Init()
        {
            if (!_internetConnector.IsConnected)
            {
                Debug.LogWarning("Purchases are not available, no internet!");
                IsSystemReady = true;

                return;
            }
            if (_storeController == null)
            {
                var module = StandardPurchasingModule.Instance();
                module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
                
                var builder = ConfigurationBuilder.Instance(module);
                builder.AddProduct(REMOVE_ADS_PRODUCT, ProductType.NonConsumable);
                
                UnityPurchasing.Initialize(this, builder);

                IsSystemReady = true;
            }
        }

        public void BuyRemoveAds()
        {
            if (_storeController != null)
            {
                _storeController.InitiatePurchase(REMOVE_ADS_PRODUCT);
            }
            else
            {
                Debug.LogWarning("IAP not initialized");
            }
        }
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensionProvider = extensions;
            
            var product = controller.products.WithID(REMOVE_ADS_PRODUCT);
            if (product != null && product.hasReceipt)
            {
                Debug.Log("Remove Ads already purchased");
                _purchasesDataController.SaveRemoveAdsPurchase();
            }
        }
        
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (purchaseEvent.purchasedProduct.definition.id == REMOVE_ADS_PRODUCT)
            {
                Debug.Log("Remove Ads already purchased");
                _purchasesDataController.SaveRemoveAdsPurchase();
            }
            
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Initialization failed: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"Initialization failed: {message}");

        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogWarning("Purchase failed: " + failureReason.ToString());
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogWarning("Purchase failed: " + failureDescription.ToString());
        }
    }
}