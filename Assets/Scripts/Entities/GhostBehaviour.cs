using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    // Necessary
    public List<Vector2> Path { private get; set; }

    public bool forward;
    public int speed { get; set; }

    private float positionIndex;
    private bool willDestroy;

    // Determine Start Position
    private void Start() => positionIndex = !forward ? Path.Count : 0;

    private void Update()
    {
        if (!willDestroy) FollowPath();
    }

    private void FollowPath()
    {
        if (forward)
        {
            positionIndex += speed * Time.deltaTime;
            if ((int)Mathf.Floor(positionIndex) >= Path.Count - 1)
            {
                StartCoroutine(Destroy());
                return;
            }
            transform.position = Path[(int)Mathf.Floor(positionIndex)];
        }
        else
        {
            positionIndex -= speed * Time.deltaTime;
            if ((int)Mathf.Floor(positionIndex) <= 1)
            {
                StartCoroutine(Destroy());
                return;
            }
            transform.position = Path[(int)Mathf.Ceil(positionIndex - 1)];
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

    public void Freeze() => speed = 0;
}