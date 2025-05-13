using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class ReloadButtonView : MonoBehaviour, IView
    {
        [SerializeField] private Button _reloadButton;

        public void Show() => _reloadButton.gameObject.SetActive(true);
        public void Hide() => _reloadButton.gameObject.SetActive(false);
        
        public event Action OnButtonClicked;
        private void Awake()
        {
            _reloadButton.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }
    }
}

