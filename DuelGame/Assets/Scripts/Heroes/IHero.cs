using UnityEngine;

namespace DuelGame
{
    public interface IHero
    {
        //public Side side { get; set; }
        public Players player { get; set; }
        public bool isDead { get; set; }
        public HeroStats hero { get; set; }
        public void ChoosSideAndPlayer(Players player, Transform trans) 
        {
            if (player == Players.Player2)
                trans.localScale = new Vector3(trans.localScale.x * (-1), trans.localScale.y, trans.localScale.z);
            //this.side = side;
            this.player = player;
        }
        public void Attack();    
        public void TakeHit(float damage, BuffEnum buffEnum);
        public void ChangeAttackStatus(bool isAttackable);
        public void GetStunned(float stunDuration);
        public void ChangeCurrHealth(float damage);
    }

    public enum Side
    {
        Left,
        Right
    }
}

