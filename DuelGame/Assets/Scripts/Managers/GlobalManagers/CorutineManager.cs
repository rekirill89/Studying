using UnityEngine;

namespace DuelGame
{
    public class CorutineManager : MonoBehaviour, IBaseManager
    {
        public GameEventManager gameEventManager { get; set; }
        public CorutineManager(GameEventManager gameEventManager)
        {
            this.gameEventManager = gameEventManager;
        }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize(GameEventManager gameEventManager)
        {
            this.gameEventManager = gameEventManager;

            this.gameEventManager.OnSceneReload += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }
    }
}

