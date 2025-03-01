using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuelGame
{
    public class GlobalBootstrap : MonoBehaviour
    {
        [SerializeField] private HeroesList _heroes;
        [SerializeField] private BuffsList _buffs;
        [SerializeField] private CorutineManager _coroutineManager;
        public BuffsManager buffsManager { get; private set; }
        public EntityFactory entityFactory { get; private set; }
        public GameEventManager gameEventManager { get; private set; }
        public CorutineManager corutineManager { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);            
            
            entityFactory = new EntityFactory(_heroes);
            gameEventManager = new GameEventManager();    
        }

        private void Start()
        {

            var buffs = Instantiate(_buffs);
            ServiceLocator.Register(entityFactory);
            ServiceLocator.Register(gameEventManager);
            corutineManager = Instantiate(_coroutineManager);
            corutineManager.Initialize(gameEventManager);
            ServiceLocator.Register(corutineManager);

            buffsManager = new BuffsManager(gameEventManager, buffs);

            for(int i = 0; i< buffs.listOfEntities.Count; i++)
            {
                Debug.Log(buffs.listOfEntities[i].buff);
            }
            SceneManager.LoadScene("BattleScene");
        }
    }
}

