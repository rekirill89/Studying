using System;
using UnityEngine;

namespace DuelGame
{
    public class SkinShopPanelPresenter : IPresenter<SkinShopView>
    {
        public event Action OnBackToMenuButtonClick;
        
        private readonly SkinShopView _skinShopView;
        private readonly SkinsController _skinsController;
        
        private SkinSlotViewPresenter _skinSlotViewPresenter1;
        private SkinSlotViewPresenter _skinSlotViewPresenter2;
        private SkinSlotViewPresenter _skinSlotViewPresenter3;

        public SkinShopPanelPresenter(SkinShopView skinShopView, SkinsController skinsController)
        {
            _skinShopView = skinShopView;
            _skinsController = skinsController;
        }
       
        public void Dispose()
        {
            _skinShopView.BackToMenuButton.OnClick -= BackToMenuButtonInvoke;
        }

        public void Initialize()
        {
            _skinSlotViewPresenter1 = new SkinSlotViewPresenter(_skinsController, _skinShopView.SkinSlotView1);
            _skinSlotViewPresenter2 = new SkinSlotViewPresenter(_skinsController, _skinShopView.SkinSlotView2);
            _skinSlotViewPresenter3 = new SkinSlotViewPresenter(_skinsController, _skinShopView.SkinSlotView3);
            
            _skinSlotViewPresenter1.Initialize();
            _skinSlotViewPresenter2.Initialize();
            _skinSlotViewPresenter3.Initialize();
            
            _skinShopView.BackToMenuButton.OnClick += BackToMenuButtonInvoke;
        }
 
        public void ShowView()
        {
            _skinShopView.Show();
        }

        private void HideView()
        {
            _skinShopView.Hide();
        }
        
        private void BackToMenuButtonInvoke()
        {
            HideView();
            OnBackToMenuButtonClick?.Invoke();
        }
    }
}