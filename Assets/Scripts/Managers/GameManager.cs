using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Button _startButton;
    // Restart
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private Button _restartButton;
    // Continue
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private Button _continueButton;
    //
    [SerializeField] private Button _reloadSceneButton;

    public static GameManager Instance;
    public event Action OnGameStarted;
    public event Action OnGameContinued;
    public event Action OnGameEnded;

    enum GameState
    {
        NotStarted,
        InProcess,
        Finished
    }

    private GameState _currentState;
    private void Awake()
    {
        Instance = this;
        _currentState = GameState.NotStarted;
    }
    private void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _restartButton.onClick.AddListener(RestartGame);
        _continueButton.onClick.AddListener(ContinueGame);
        _reloadSceneButton.onClick.AddListener(RestartGame);

        GameStartPanel();
    }

    public void GameStartPanel()
    {
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
        OnGameStarted?.Invoke();
    }    
    private void ContinueGame()
    {
        _currentState = GameState.InProcess;
        _continuePanel.SetActive(false);
        OnGameContinued?.Invoke();
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
