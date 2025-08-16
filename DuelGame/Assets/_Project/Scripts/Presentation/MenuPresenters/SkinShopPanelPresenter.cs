using System;
using UnityEngine;

namespace DuelGame
{
    public class SkinShopPanelPresenter : IPresenter<SkinShopPanelView>
    {
        public event Action OnBackToMenuButtonClick;
        
        private readonly SkinShopPanelView _skinShopPanelView;
        private readonly SkinsController _skinsController;
        
        private SkinSlotViewPresenter _skinSlotViewPresenter1;
        private SkinSlotViewPresenter _skinSlotViewPresenter2;
        private SkinSlotViewPresenter _skinSlotViewPresenter3;

        public SkinShopPanelPresenter(SkinShopPanelView skinShopPanelView, SkinsController skinsController)
        {
            _skinShopPanelView = skinShopPanelView;
            _skinsController = skinsController;
        }
       
        public void Dispose()
        {
            _skinShopPanelView.BackToMenuButton.OnClick -= BackToMenuButtonInvoke;
        }

        public void Initialize()
        {
            _skinSlotViewPresenter1 = new SkinSlotViewPresenter(_skinsController, _skinShopPanelView.SkinSlotView1);
            _skinSlotViewPresenter2 = new SkinSlotViewPresenter(_skinsController, _skinShopPanelView.SkinSlotView2);
            _skinSlotViewPresenter3 = new SkinSlotViewPresenter(_skinsController, _skinShopPanelView.SkinSlotView3);
            
            _skinSlotViewPresenter1.Initialize();
            _skinSlotViewPresenter2.Initialize();
            _skinSlotViewPresenter3.Initialize();
            
            _skinShopPanelView.BackToMenuButton.OnClick += BackToMenuButtonInvoke;
        }
 
        public void ShowView()
        {
            _skinShopPanelView.Show();
        }

        private void HideView()
        {
            _skinShopPanelView.Hide();
        }
        
        private void BackToMenuButtonInvoke()
        {
            HideView();
            OnBackToMenuButtonClick?.Invoke();
        }
    }
}