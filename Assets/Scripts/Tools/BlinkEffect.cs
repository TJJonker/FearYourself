using UnityEngine;
using TMPro;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    [SerializeField] private float speed;

    private TextMeshProUGUI tmpro;
    private float sinX;

    private void Awake() => tmpro = GetComponent<TextMeshProUGUI>();

    private void Update()
    {
        var Delta = minColor - maxColor;

        var color = Mathf.Sin(sinX + Mathf.PI/2) * (Delta / 2) + minColor - (Delta / 2);
        tmpro.color = color;

        sinX += speed * Time.deltaTime;
    }
}
