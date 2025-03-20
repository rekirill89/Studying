using UnityEngine;

namespace DuelGame
{
    public class EntityFactory
    {
        private readonly HeroesList _heroes; 
        
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
    }
}

