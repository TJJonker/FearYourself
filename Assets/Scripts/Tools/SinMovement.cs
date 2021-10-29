using UnityEngine;

[System.Serializable]
public class SinMover
{
    public enum modes { sin, cos }

    [System.NonSerialized] public float X;
    public modes mode;
    public float speed;
    public float amplitude;
    public Vector2 direction;
    public float offset;

    public Vector2 Next()
    {
        // Calculate Sin
        float movement = mode == modes.cos ? Mathf.Cos(X + offset * Mathf.PI) : Mathf.Sin(X + offset * Mathf.PI);
        // Multiply amplitude
        movement *= amplitude;
        // Add increment
        X += speed * Time.deltaTime;
        // return new position
        return direction * movement;
    }
}

public class SinMovement : MonoBehaviour
{
    public SinMover[] Movers = new SinMover[0];

    private void Update()
    {
        for (int i = 0; i < Movers.Length; i++)
            Move(Movers[i].Next());
    }

    private void Move(Vector2 pos) => gameObject.transform.position += (Vector3)pos * Time.deltaTime;
}