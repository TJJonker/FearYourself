using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private LayerMask FinishLayer;
    private GameObject Level;

    private const float GROUND_DISTANCE = .2f;

    void Start() => Level = GameObject.FindWithTag("Level");

    public bool CheckForFinish()
    {
        // Determine start position
        Vector2 startPos1 = new Vector2(transform.position.x + transform.localScale.x / 2f,
                                        transform.position.y - transform.localScale.y / 2f);
        Vector2 startPos2 = new Vector2(transform.position.x - transform.localScale.x / 2f,
                                        transform.position.y - transform.localScale.y / 2f);
        // Create two raycasts
        RaycastHit2D hit1 = Physics2D.Raycast(startPos1, Vector2.down, GROUND_DISTANCE, FinishLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(startPos2, Vector2.down, GROUND_DISTANCE, FinishLayer);
        // Return true if ground hit, else false
        if (hit1.collider || hit2.collider) return true;
        return false;
    }
}
