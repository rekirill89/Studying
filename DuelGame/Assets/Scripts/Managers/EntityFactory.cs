using UnityEngine;

namespace DuelGame
{
    public class EntityFactory
    {
        private HeroesList _heroes; 
        public EntityFactory(HeroesList heroes)
        {
            _heroes = heroes;
        }

        public BaseHero SpawnRandomHero(Transform trans)
        {
            var entity = _heroes.GetRandomEntity();
            var x = Object.Instantiate(entity.heroScript, trans);
            x.Initialize(entity.heroStats);
            return x;
        }

        public GameObject CreateObject(GameObject obj, Transform trans)
        {
            var x = Object.Instantiate(obj, trans);
            return x;
        }
        public void DestroyObject(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}

