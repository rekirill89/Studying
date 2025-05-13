using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    public class RestartPanelView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Button _restartButton;
        
        public void Show() => _root.SetActive(true);
        public void Hide() => _root.SetActive(false);
        
        public  event Action OnButtonClicked;

        private void Awake()
        {
            _restartButton.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }
    }   
}
