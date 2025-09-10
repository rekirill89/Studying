using System;
using DG.Tweening;
using UnityEngine;

namespace DuelGame
{
    public class BattleFinishPanelPresenter : IPresenter<BattleFinishView>
    {
        private readonly BattleManager _battleManager;
        private readonly BattleFinishView _battleFinishView;
        
        private readonly float _alphaMaxTarget = 1f;
        private readonly float _alphaMinTarget = 0f;

        private readonly float _halfDuration = 3.5f;
        
        private Tween _fadeTween;
        
        public BattleFinishPanelPresenter(
            BattleManager battleManager, 
            BattleFinishView battleFinishView)
        {
            _battleManager = battleManager; 
            _battleFinishView = battleFinishView;
        }

        public void Initialize()
        {
            _battleManager.OnPlayerDead += OnBattleFinishHandler;
        }

        public void Dispose()
        {
            _battleManager.OnPlayerDead -= OnBattleFinishHandler;
            
            _fadeTween?.Kill();
        }

        public void ShowView(Players? playerwholost)
        {
            _battleFinishView.transform.SetAsLastSibling();
            Debug.Log($"Showing {GetType()}");
            _battleFinishView.Show();
            
            OnBattleFinishHandler(playerwholost);
        }
        
        private void OnBattleFinishHandler(Players? playerwholost)
        {
            if (playerwholost == Players.Player1)
                DoFadeToCanvasGroup(_battleFinishView.YouDiedView.CanvasGroup);
            
            else if (playerwholost == Players.Player2)
                DoFadeToCanvasGroup(_battleFinishView.EnemyFelledView.CanvasGroup);
        }

        private void DoFadeToCanvasGroup(CanvasGroup canvasGroup)
        {
            Debug.Log("Showing finish battle screen");
            _fadeTween?.Kill();
            
            _fadeTween = DOTween.Sequence()
                .Append(canvasGroup.DOFade(_alphaMaxTarget, _halfDuration))
                .Append(canvasGroup.DOFade(_alphaMinTarget, _halfDuration)
                    .OnComplete(() => _battleFinishView.Hide()));
        }
    }
}