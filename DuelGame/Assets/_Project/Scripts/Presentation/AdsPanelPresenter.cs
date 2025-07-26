using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class AdsPanelPresenter : IPresenter<AdsPanelView>
    {
        private readonly BattleManager _battleManager;
        private readonly AdsPanelView _view;
        private readonly IAdService _adService;

        public AdsPanelPresenter(BattleManager battleManager, AdsPanelView view, IAdService adService)
        {
            _battleManager = battleManager;
            _view = view;
            _adService = adService;
        }
        
        public void Initialize()
        {
            _battleManager.OnBattleFinish += ShowView;
            _battleManager.BattleStateModel.OnStateChanged += StateChangedHandler;
            _view.AcceptButton.OnClick += OnAcceptButtonClickedHandler;
            _view.CancelButton.OnClick += OnCancelButtonClickedHandler;
            _adService.OnRewardedAdClosed += _battleManager.ContinueBattle;
            _adService.OnInterstiateAdClosed += HideView;        
        }
        
        public void Dispose()
        {
            _battleManager.OnBattleFinish -= ShowView;
            _battleManager.BattleStateModel.OnStateChanged -= StateChangedHandler;
            _view.AcceptButton.OnClick -= OnAcceptButtonClickedHandler;
            _view.CancelButton.OnClick -= OnCancelButtonClickedHandler;
            _adService.OnRewardedAdClosed -= _battleManager.ContinueBattle;
            _adService.OnInterstiateAdClosed -= HideView;
        }
        
        public void ShowView(Players? playerWhoLost)
        {
            _view.transform.SetAsLastSibling();
            if (playerWhoLost == Players.Player1)
            {
                _view.Show();
            }
            else
            {
                _adService.ShowInterstitialAd();
            }
        }

        private void HideView()
        {
            _view.Hide();
        }
        
        private void StateChangedHandler(BattleState state)
        {
            if(state == BattleState.Continued)
                _view.Hide();
        }
        
        private void OnAcceptButtonClickedHandler()
        {
            _adService.ShowRewardedAd();
        }
        
        private void OnCancelButtonClickedHandler()
        {
            _adService.ShowInterstitialAd();
        }
    }
}