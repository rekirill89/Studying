using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private BuffsList _buffs;
        [SerializeField] private HeroesList _heroes;

        public override void InstallBindings()
        {
            Debug.Log("Project installer created");

            var buffsList = Instantiate(this._buffs);
            var heroesList = Instantiate(this._heroes);

            Container.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            Container.Bind<HeroesList>().FromInstance(heroesList).AsSingle();

            Container.Bind<EntityFactory>().AsSingle();
        }
    }
}
