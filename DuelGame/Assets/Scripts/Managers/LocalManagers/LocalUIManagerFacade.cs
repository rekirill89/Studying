using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class LocalUIManagerFacade : MonoBehaviour
    {
        // Start
        [SerializeField] public GameObject startPanel;
        [SerializeField] public Button startButton;
        // Restart
        [SerializeField] public GameObject restartPanel;
        [SerializeField] public Button restartButton;
        // Continue
        [SerializeField] public GameObject continuePanel;
        [SerializeField] public Button continueButton;
        // Reload
        [SerializeField] public Button reloadSceneButton;
    }
}

