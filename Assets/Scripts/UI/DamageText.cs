using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI _damageText;
    private float _interval = 0.005f;
    private float _timer = 1.5f;

    private void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        transform.position += Vector3.up * Time.fixedDeltaTime;

        _damageText.color = new Color(_damageText.color.r, _damageText.color.g, _damageText.color.b, _damageText.color.a - _interval);
        _interval += 0.0005f;

        _timer -= Time.fixedDeltaTime;
        if(_timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float damage)
    {
        _damageText.text = (-damage).ToString();
    }
}
