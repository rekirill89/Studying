using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class NextSceneLoader : IInitializable
    {
        public void Initialize()
        {
            Debug.Log("Loading next scene...");
            SceneManager.LoadScene("BattleScene");
        }
    }
}

