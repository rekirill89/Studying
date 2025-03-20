using System;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class GameStateController : IInitializable, IDisposable
    {
        private readonly BattleManager _localBattleManager;
        private readonly BattleSceneUIManager _localUIManager;

        private GameState _currentState = GameState.NotStarted;
        
        public GameStateController(BattleManager localBattleManager, BattleSceneUIManager localUIManager)
        {
            _localBattleManager = localBattleManager;
            _localUIManager = localUIManager;

            _localUIManager.OnGameStartButtonPressed += StartGameController;
            _localUIManager.OnGameContinueButtonPressed += ContinueGameController;
            _localBattleManager.OnBattleFinish += FinishBattleController;

        }
        public void Initialize()
        {
            _currentState = GameState.InProcess;
            _localUIManager.GameStartPanel();
        }

        private void StartGameController()
        {
            _localBattleManager.RunBattle();
        }

        private void ContinueGameController()
        {
            _currentState = GameState.InProcess;
            _localBattleManager.ContinueBattle();
        }

        private void FinishBattleController(Players playerWhoLost)
        {
            _currentState = GameState.Finished;
            _localUIManager.BattleFinished(playerWhoLost);
        }

        public void Dispose()
        {
            _localUIManager.OnGameStartButtonPressed -= StartGameController;
            _localUIManager.OnGameContinueButtonPressed -= ContinueGameController;
            _localBattleManager.OnBattleFinish -= FinishBattleController;
        }
    }
    internal enum GameState
    {
        NotStarted,
        InProcess,
        Finished
    }
}

