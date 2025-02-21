using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _damageText;
    [SerializeField] private Transform _textPositionPl1;
    [SerializeField] private Transform _textPositionPl2;


    [SerializeField] private Transform _iconPositionPl1;
    [SerializeField] private Transform _iconPositionPl2;


    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDamageText(float damage, Player player)
    {
        Transform trans = player == Player.Player1 ? _textPositionPl1 : _textPositionPl2;
        var x = Instantiate(_damageText, trans);
        x.transform.localScale = new Vector3(0.09f, 0.09f, 1);
        
        x.GetComponent<DamageText>().Initialize(damage);
    }

    public IEnumerator SpawnBuffIcon(GameObject buffObj, float duration, Player player)
    {
        Debug.Log(nameof(SpawnBuffIcon));

        Transform trans = player == Player.Player1 ? _textPositionPl1 : _textPositionPl2;
        var x = Instantiate(buffObj, trans);

        yield return new WaitForSeconds(duration);
        Destroy(x);
    }
}
