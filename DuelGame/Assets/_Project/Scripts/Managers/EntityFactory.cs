using UnityEngine;

namespace DuelGame
{
    public class EntityFactory
    {
        private readonly HeroesList _heroes; 
        private readonly BuffsList _buffsList; 

        
        public EntityFactory(HeroesList heroes, BuffsList buffsList)
        {
            _heroes = heroes;
            _buffsList = buffsList;
        }

        public BaseHero SpawnRandomHero(Transform trans)
        {
            var entity = _heroes.GetRandomEntity();
            var x = Object.Instantiate(entity.heroScript, trans);
            x.Initialize(entity.heroStats, _buffsList);
            return x;
        }

        public BaseHero SpawnHeroByEnum(Transform trans, HeroEnum heroEnum)
        {
            var entity = _heroes.GetHeroEntityByEnum(heroEnum);
            var x = Object.Instantiate(entity.heroScript, trans);
            x.Initialize(entity.heroStats, _buffsList);
            return x;
        }
    }
}

