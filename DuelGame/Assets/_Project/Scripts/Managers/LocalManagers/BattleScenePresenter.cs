using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BattleScenePresenter : IInitializable, IDisposable
    {
        private readonly BattleManager _localBattleManager;
        private readonly BattleSceneUIManager _localUIManager;
        private readonly SceneLoaderManager _sceneLoaderManager;
        
        public BattleScenePresenter(BattleManager localBattleManager, BattleSceneUIManager localUIManager, SceneLoaderManager sceneLoaderManager)
        {
            _localBattleManager = localBattleManager;
            _localUIManager = localUIManager;
            _sceneLoaderManager = sceneLoaderManager;
            
            _localUIManager.OnGameStartButtonPressed += StartGame;
            _localUIManager.OnGameContinueButtonPressed += ContinueGameController;
            _localUIManager.OnGameRestartButtonPressed += RestartGame;
            
            _localBattleManager.OnPlayersSpawned += PlayersSpawned;
            _localBattleManager.OnBattleFinish += FinishBattleController;
        }

        public void Initialize()
        {
            _localUIManager.SetVisibleStartPanel(true);
        }

        public void Dispose()
        {
            _localUIManager.OnGameStartButtonPressed -= StartGame;
            _localUIManager.OnGameContinueButtonPressed -= ContinueGameController;
            _localUIManager.OnGameRestartButtonPressed -= RestartGame;
            
            _localBattleManager.OnPlayersSpawned -= PlayersSpawned;
            _localBattleManager.OnBattleFinish -= FinishBattleController;
        }
        
        private void StartGame()
        {
            _localBattleManager.RunBattle();
        }
        
        private void RestartGame()
        {
            _sceneLoaderManager.LoadBattleScene();
        }    
        
        private void ContinueGameController()
        {
            _localBattleManager.ContinueBattle();
        }

        private void FinishBattleController(Players playerWhoLost)
        {
            if(playerWhoLost == Players.Player2)
                _localUIManager.SetVisibleContinuePanel(true);
            else if(playerWhoLost == Players.Player1)
                _localUIManager.SetVisibleRestartPanel(true);
        }
        
        private void PlayersSpawned(BattleState state)
        {
            if (state == BattleState.Started)
                _localUIManager.SetVisibleStartPanel(false);
            else if (state == BattleState.Continued)
                _localUIManager.SetVisibleContinuePanel(false);
        }
    }
}

