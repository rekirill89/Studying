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

        public void InitializeStateUIElements(ScreenCanvasFacade screenCanvasFacade, HUDCanvasFacade hudCanvasFacade)
        {
            _startPanel = screenCanvasFacade.StartPanelFacade.StartPanel;
            _startButton = screenCanvasFacade.StartPanelFacade.StartButton;

            _restartPanel = screenCanvasFacade.RestartPanelFacade.RestartPanel;
            _restartButton = screenCanvasFacade.RestartPanelFacade.RestartButton;

            _continuePanel = screenCanvasFacade.ContinuePanelFacade.ContinuePanel;
            _continueButton = screenCanvasFacade.ContinuePanelFacade.ContinueButton;

            _reloadSceneButton = hudCanvasFacade.ReloadButtonFacade.ReloadSceneButton;

            _startButton.onClick.AddListener(() => OnGameStartButtonPressed?.Invoke());
            _restartButton.onClick.AddListener(() => OnGameRestartButtonPressed?.Invoke());
            _continueButton.onClick.AddListener(() => OnGameContinueButtonPressed?.Invoke());
            _reloadSceneButton.onClick.AddListener(() => OnGameRestartButtonPressed?.Invoke());
        }
    }       
}

