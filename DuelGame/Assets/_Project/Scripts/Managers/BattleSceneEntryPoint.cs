using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DuelGame
{
    public class BattleSceneEntryPoint : IInitializable, IDisposable
    {
        private BattleManager _battleManager;
        private BattleSessionContext _battleSessionContext;
        private UIFactory _uiFactory;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        public BattleSceneEntryPoint(
            BattleManager battleManager, 
            BattleSessionContext battleSessionContext, 
            UIFactory uiFactory)
        {
            _battleManager = battleManager;
            _battleSessionContext = battleSessionContext;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            Entry().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        private async UniTask Entry()
        {
            if (!await CheckSystemsInit())
            {
                Debug.Log("Systems not initialized");
                return;
            }
            _battleManager.InvokeStartBattle();
        }
        
        private async UniTask<bool> CheckSystemsInit()
        {
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(20), cancellationToken: _cts.Token);
            await UniTask.WaitUntil(
                () => (_uiFactory.IsSystemReady &&  _battleSessionContext.IsSystemReady) || 
                      timeout.Status == UniTaskStatus.Succeeded || timeout.Status == UniTaskStatus.Faulted,
                cancellationToken: _cts.Token);
            if(timeout.Status == UniTaskStatus.Succeeded || timeout.Status == UniTaskStatus.Faulted)
                return false;
            
            return true;
        }
    }
}