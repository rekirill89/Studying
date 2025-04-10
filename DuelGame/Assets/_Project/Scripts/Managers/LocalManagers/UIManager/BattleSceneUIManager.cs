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
        public event Action OnGameRestartButtonPressed;

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

        public BattleSceneUIManager(
            StartPanelFacade startPanelFacade, 
            RestartPanelFacade restartPanelFacade,
            ContinuePanelFacade continuePanelFacade,
            ReloadButtonFacade reloadButtonFacade)
        {
            InitializeStateUIElements(startPanelFacade, restartPanelFacade, continuePanelFacade, reloadButtonFacade);
        }
        
        public void SetVisibleStartPanel(bool isVisible)
        {
            _startPanel.SetActive(isVisible);
        }

        public void SetVisibleContinuePanel(bool isVisible)
        {
            _continuePanel.SetActive(isVisible);
        }

        public void SetVisibleRestartPanel(bool isVisible)
        {
            _restartPanel.SetActive(isVisible);
        }

        private void InitializeStateUIElements(
            StartPanelFacade startPanelFacade, 
            RestartPanelFacade restartPanelFacade,
            ContinuePanelFacade continuePanelFacade,
            ReloadButtonFacade reloadButtonFacade)
        {
            _startPanel = startPanelFacade.StartPanel;
            _startButton = startPanelFacade.StartButton;

            _restartPanel = restartPanelFacade.RestartPanel;
            _restartButton = restartPanelFacade.RestartButton;

            _continuePanel = continuePanelFacade.ContinuePanel;
            _continueButton = continuePanelFacade.ContinueButton;

            _reloadSceneButton = reloadButtonFacade.ReloadSceneButton;

            _startButton.onClick.AddListener(() => OnGameStartButtonPressed?.Invoke());
            _restartButton.onClick.AddListener(() => OnGameRestartButtonPressed?.Invoke());
            _continueButton.onClick.AddListener(() => OnGameContinueButtonPressed?.Invoke());
            _reloadSceneButton.onClick.AddListener(() => OnGameRestartButtonPressed?.Invoke());
        }
    }       
}

