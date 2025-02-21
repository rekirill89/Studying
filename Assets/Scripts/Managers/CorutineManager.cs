using UnityEngine;

public class CorutineManager : MonoBehaviour
{
    public static CorutineManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}
