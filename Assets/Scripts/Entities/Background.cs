using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float speed;
    [SerializeField] private float amplitude;
    [SerializeField] private Color startColor;

    private float sinX;

    void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void Update()
    {
        var sin = (Mathf.Sin(sinX) * amplitude) / 255;
        _spriteRenderer.color = startColor + new Color(sin, sin, sin);

        sinX += speed * Time.deltaTime;
    }

}
