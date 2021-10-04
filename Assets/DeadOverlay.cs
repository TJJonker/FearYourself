using System.Collections.Generic;
using UnityEngine;

public class DeadOverlay : MonoBehaviour
{
    List<SpriteRenderer> children = new List<SpriteRenderer>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            children.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
    }

    public void EnableDead()
    {
        foreach (SpriteRenderer s in children)
            s.enabled = true;
    }

    public void DisableDead()
    {
        foreach (SpriteRenderer s in children)
            s.enabled = false;
    }
}
