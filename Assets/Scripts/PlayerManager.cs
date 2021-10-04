using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private LayerMask FinishLayer;
    private GameObject Level;
    private LevelManager LevelManager;

    private const float GROUND_DISTANCE = .2f;

    void Start()
    {
        Level = GameObject.FindWithTag("Level");
        LevelManager = Level.GetComponent<LevelManager>();
    }

    void Update()
    {
        CheckForFinish();        
    }

    private void CheckForFinish()
    {
        var StartPos = new Vector2(transform.position.x, transform.position.y - transform.localScale.y/2f);
        RaycastHit2D hit = Physics2D.Raycast(StartPos, Vector2.down, GROUND_DISTANCE, FinishLayer);
        if(hit.collider != null) LevelManager.Finish();

    }
}
