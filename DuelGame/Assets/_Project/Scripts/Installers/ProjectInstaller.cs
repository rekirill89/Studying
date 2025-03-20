using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private BuffsList buffs;
        [SerializeField] private HeroesList heroes;
        //[SerializeField] private CorutineManager corutineManager;

        public override void InstallBindings()
        {
            Debug.Log("Project installer created");

            var buffsList = Instantiate(this.buffs);
            var heroesList = Instantiate(this.heroes);
            //var corutineManager = Instantiate(this.corutineManager);

            Container.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            Container.Bind<HeroesList>().FromInstance(heroesList).AsSingle();

            Container.Bind<EntityFactory>().AsSingle();
            //Container.Bind<CorutineManager>().FromInstance(corutineManager).AsSingle();
        }
    }
}
