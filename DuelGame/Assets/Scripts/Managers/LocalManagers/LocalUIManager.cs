using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DuelGame
{
    public class LocalUIManager : IBaseManager 
    {
        // Start
        private GameObject _startPanel;
        private Button _startButton;
        // Restart
        private GameObject _restartPanel;
        private Button _restartButton;
        // Continue
        private GameObject _continuePanel;
        private Button _continueButton;
        // Reload
        private Button _reloadSceneButton;

        public GameEventManager gameEventManager { get; set; }

        private GameState _currentState;

        public LocalUIManager(GameEventManager gameEventManager)
        {
            this.gameEventManager = gameEventManager;

            gameEventManager.OnBattleFinished += BattleFinished;            
        }
        private void BattleFinished(Players playerWhoLost)
        {
            Action t = playerWhoLost == Players.Player1 ? () => GameFinishPanel() : () => GameContinuePanel();
            t.Invoke();
        }
        public void GameStartPanel()
        {
            Debug.Log("Game initialized");
            _startPanel.SetActive(true);
        }
        public void GameContinuePanel()
        {
            _continuePanel.SetActive(true);
        }    
        public void GameFinishPanel()
        {
            _currentState = GameState.Finished;
            _restartPanel.SetActive(true);
        }

        private void StartGame()
        {
            _currentState = GameState.InProcess;
            _startPanel.SetActive(false);
            gameEventManager.BattleStartInvoke();
        }    
        private void ContinueGame()
        {
            _currentState = GameState.InProcess;
            _continuePanel.SetActive(false);
            gameEventManager.BattleContinueInvoke();
        }
        private void RestartGame()
        {
            gameEventManager.SceneReloadInvoke();
            SceneManager.LoadScene(0);
        }

        public void InitializeStateUIElements(LocalUIManagerFacade facade)
        {
            _startPanel = facade.startPanel;
            _startButton = facade.startButton;

            _restartPanel = facade.restartPanel;
            _restartButton = facade.restartButton;

            _continuePanel = facade.continuePanel;
            _continueButton = facade.continueButton;

            _reloadSceneButton = facade.reloadSceneButton;

            _startButton.onClick.AddListener(StartGame);
            _restartButton.onClick.AddListener(RestartGame);
            _continueButton.onClick.AddListener(ContinueGame);
            _reloadSceneButton.onClick.AddListener(RestartGame);
        }
        public Action OnReadyMethod()
        {
            return GameStartPanel;
        }
    }       
    enum GameState
    {
        NotStarted,
        InProcess,
        Finished
    }
}

