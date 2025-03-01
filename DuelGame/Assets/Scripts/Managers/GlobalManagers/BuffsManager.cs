using UnityEngine;
using System.Linq;

namespace DuelGame
{
    public class BuffsManager : IBaseManager
    {        
        public GameEventManager gameEventManager { get; set; }

        private BuffsList _buffs;

        private BaseHero _player1;
        private BaseHero _player2;

        public BuffsManager(GameEventManager gameEventManager, BuffsList buffs)
        {
            this.gameEventManager = gameEventManager;
            _buffs = buffs; 

            this.gameEventManager.OnPlayersInstantiated += PlayersInstantiatedHandler;
        }
        private void PlayersInstantiatedHandler(BaseHero player1, BaseHero player2)
        {
            _player1 = player1;
            _player2 = player2;

            _player1.OnBuffGot += BuffApply;
            _player2.OnBuffGot += BuffApply;
        }

        private void BuffApply(BuffEnum buffEnum, BaseHero player)
        {
            if(buffEnum != BuffEnum.None)
            {
                var buffEntity = _buffs.listOfEntities.First(x => x.buff == buffEnum);
                Buff buff = Object.Instantiate(buffEntity.buffScript);
                buff.DoBuff(player);
                Object.Destroy(buff, buff.buffDuration + 0.1f);

                gameEventManager.BuffAppliedInvoke(buffEntity.sp.sprite, buff.buffDuration, player.player);
            }
        }
    }
}

