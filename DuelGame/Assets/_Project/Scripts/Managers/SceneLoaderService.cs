using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuelGame
{
    public class SceneLoaderManager
    {
        private const string BATTLE_SCENE = "BattleScene";
        
        public void LoadBattleScene()
        {
            SceneManager.LoadScene(BATTLE_SCENE);
        }
    }
}