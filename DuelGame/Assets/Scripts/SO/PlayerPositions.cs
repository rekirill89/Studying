using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPositions", menuName = "Scriptable Objects/PlayerPositions")]
public class PlayerPositions : ScriptableObject
{
    [SerializeField] GameObject _pos1;
    [SerializeField] GameObject _pos2;
}
