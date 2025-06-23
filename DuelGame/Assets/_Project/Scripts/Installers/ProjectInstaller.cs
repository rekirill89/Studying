using UnityEditor.Overlays;
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

            var buffsList = Instantiate(_buffs);
            var heroesList = Instantiate(_heroes);

            Container.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            Container.Bind<HeroesList>().FromInstance(heroesList).AsSingle();

            Container.Bind<EntityFactory>().AsSingle();
            Container.Bind<SceneLoaderService>().AsSingle();
            
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<BattleDataCache>().AsSingle();
        }
    }
}