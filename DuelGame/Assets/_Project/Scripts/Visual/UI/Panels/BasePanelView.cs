using System;
using UnityEngine;

namespace DuelGame
{
    public class BasePanelView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        
        public void Show() => _root.SetActive(true);
        
        public void Hide() => _root.SetActive(false);
    }
}