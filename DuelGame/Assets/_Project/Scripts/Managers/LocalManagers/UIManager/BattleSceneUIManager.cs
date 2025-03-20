using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace DuelGame
{
    public class BattleSceneUIManager
    {
        public event Action OnGameStartButtonPressed;
        public event Action OnGameContinueButtonPressed;

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


        public BattleSceneUIManager(BattleSceneUIManagerFacade facade)
        {
            InitializeStateUIElements(facade);
        }

        public void BattleFinished(Players playerWhoLost)
        {
            Action t = playerWhoLost == Players.Player1 ? () => GameFinishPanel() : () => GameContinuePanel();
            t.Invoke();
        }
        public void GameStartPanel()
        {
            Debug.Log("Game initialized");
            _startPanel.SetActive(true);
        }
        
        
        private void GameContinuePanel()
        {
            _continuePanel.SetActive(true);
        }   
        
        private void GameFinishPanel()
        {
            _restartPanel.SetActive(true);
        }

        private void StartGame()
        {
            OnGameStartButtonPressed?.Invoke();
            _startPanel.SetActive(false);
        }   
        
        private void ContinueGame()
        {
            _continuePanel.SetActive(false);
            OnGameContinueButtonPressed?.Invoke();
        }
        
        private void RestartGame()
        {
            SceneManager.LoadScene("BattleScene");
        }

        private void InitializeStateUIElements(BattleSceneUIManagerFacade facade)
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
    }       
}

