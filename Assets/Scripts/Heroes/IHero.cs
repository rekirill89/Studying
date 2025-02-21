using UnityEngine;

public interface IHero
{
    public Side side { get; set; }
    public Player player { get; set; }
    public bool isDead { get; set; }
    public HeroStats hero { get; set; }
    public void ChoosSideAndPlayer(Side side, Player player, Transform trans) 
    {
        if (side == Side.Right)
            trans.localScale = new Vector3(trans.localScale.x * (-1), trans.localScale.y, trans.localScale.z);
        this.side = side;
        this.player = player;
    }
/*    public void ChoosePlayer(Player player)
    {
        this.player = player;
    }*/
    public void Attack();    
    public void TakeHit(float damage);
    public void ChangeAttackStatus(bool isAttackable);
    public void GetStunned(float stunDuration);
    public void ChangeCurrHealth(float damage);
}

public enum Side
{
    Left,
    Right
}
