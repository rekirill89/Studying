using UnityEngine;

namespace DuelGame
{
    public class LocalUIEffectsFacade : MonoBehaviour
    {
        [SerializeField] public DamageText damageText;

        [SerializeField] public Transform parentForTexts;
        [SerializeField] public Transform textPositionPl1;
        [SerializeField] public Transform textPositionPl2;

        [SerializeField] public SpriteRenderer iconPlayer1SP;
        [SerializeField] public SpriteRenderer iconPlayer2SP;
    }
}

