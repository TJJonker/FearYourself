using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    private TrailRenderer trail;
    private SpriteRenderer spriteRenderer;
    public LevelManager levelManager { private get; set; }

    public List<Vector2> Path {private get; set;}

    private float DestroyTimer = 0;
    public bool WillDestroy = false;

    private float positionIndex;

    private int OriginalpositionIndexIncrement = 50;
    private int positionIndexIncrement;

    public int PositionIndexIncrement
    {
        get { return positionIndexIncrement; }
        // PositionIndexIncrement will gain value, as long as the value is not -1, then it will gain a preset value
        set { positionIndexIncrement = value == -1 ? OriginalpositionIndexIncrement : value; }
    }

    public bool Forward { get; set; } = false;

    private void Awake()
    {
        PositionIndexIncrement = OriginalpositionIndexIncrement;
    }

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(!Forward) positionIndex = Path.Count;
        
    }

    void Update()
    {
        if(!WillDestroy) FollowPath();
        if(WillDestroy) DestroySequence();
    }

    private void FollowPath()
    {
        if(Forward)
        {
            positionIndex += PositionIndexIncrement * Time.deltaTime;
            transform.position = Path[(int)Mathf.Floor(positionIndex)];
            if((int)Mathf.Floor(positionIndex) >= Path.Count - 1) DestroyGhost();
        } else {
            positionIndex -= PositionIndexIncrement * Time.deltaTime;
            transform.position = Path[(int)Mathf.Floor(positionIndex)];
            if((int)Mathf.Ceil(positionIndex) <= 1) DestroyGhost();
        }
    }

    private void DestroyGhost() 
    {
        WillDestroy = true;
        spriteRenderer.color = Color.clear;
    }

    private void DestroySequence()
    {
        DestroyTimer += Time.deltaTime;
        if (DestroyTimer > trail.time)
        {
            levelManager.RemoveGhost(gameObject);
            Destroy(gameObject);
        }
    }
}
