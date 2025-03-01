using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class GameEventManager
    {
        public event Action<Players> OnBattleFinished;
        public event Action OnBattleStart;
        public event Action OnBattleContinue;
        public event Action<BaseHero, BaseHero> OnPlayersInstantiated;
        public event Action<Sprite, float, Players> OnBuffApplied;
        public event Action OnSceneReload;

        
        public void BattleFinishedInvoke(Players player)
        {
            OnBattleFinished?.Invoke(player);
        }
        public void BattleStartInvoke()
        {
            OnBattleStart?.Invoke();
        }
        public void BattleContinueInvoke()
        {
            OnBattleContinue?.Invoke();
        }
        public void BuffAppliedInvoke(Sprite img, float duration, Players player)
        {
            OnBuffApplied?.Invoke(img, duration, player);
        }
        public void PlayersInstantiatedInvoke(BaseHero hero1, BaseHero hero2)
        {
            OnPlayersInstantiated?.Invoke(hero1, hero2);
        }
        public void SceneReloadInvoke()
        {
            OnSceneReload?.Invoke();
        }
    }
}

