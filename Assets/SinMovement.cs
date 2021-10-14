using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private Vector3 position;
    private float sinX;
    [SerializeField] private float sinIncrement;
    [SerializeField] private float amplitude;
    [SerializeField] private Vector3 direction;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        var sin = Mathf.Sin(sinX) * amplitude;
        transform.localPosition = position + sin * direction;
        Debug.Log(position);
        sinX += sinIncrement;
    }
}
