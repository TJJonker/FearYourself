using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    // Necessary
    public List<Vector2> Path { private get; set; }
    public bool forward;
    public int speed;

    private float positionIndex;
    private bool willDestroy;

    // Determine Start Position
    void Start() => positionIndex = !forward ? Path.Count : 0;

    void Update()
    {
        if (!willDestroy) FollowPath();
    }

    private void FollowPath()
    {
        if (forward)
        {
            positionIndex += speed * Time.deltaTime;
            transform.position = Path[(int)Mathf.Floor(positionIndex)];
            if ((int)Mathf.Floor(positionIndex) >= Path.Count - 1) StartCoroutine(Destroy());
        }
        else
        {
            positionIndex -= speed * Time.deltaTime;
            Debug.Log(Path.Count);
            transform.position = Path[(int)Mathf.Ceil(positionIndex)];
            if ((int)Mathf.Floor(positionIndex) <= 1) StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        // Disable movement
        willDestroy = true;
        // Make ghost invisible
        GetComponent<SpriteRenderer>().color = Color.clear;
        // Disable Hitbox
        GetComponent<BoxCollider2D>().isTrigger = true;
        // Trigger GameEvent
        GameEvents.current.OnGhostDestroy(gameObject);
        // Wait till trail is gone
        yield return new WaitForSeconds(GetComponent<TrailRenderer>().time);
        // Destroy GameObject
        Destroy(gameObject);
    }
}
