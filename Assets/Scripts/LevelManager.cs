using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{



    [Header("Overlays")]
    [SerializeField] private GameObject DeadOverlay;
        


    [Header("Winning")]
    [SerializeField] private float Increment = .03f;

    // Playing states







    // Ghost spawning
    private List<GameObject> ghosts = new List<GameObject>();
    float GhostSpawnTimer = 0;



    private void Start()
    {



    }

    private void Update()
    {
        if(State == States.Dead) CheckDeathSequence();
        if (State == States.Finished) return;
    }













    public void Dead()
    {
        // Enable Dead overlay
        DeadOverlay.GetComponent<DeadOverlay>().EnableDead();

        // Set LevelManager to Dead
        State = States.Dead;

        // Send all ghosts back to endpoint if they exist
        foreach (GameObject g in ghosts)
            g.GetComponent<GhostBehaviour>().Forward = true;

        // Disable player
        player.SetActive(false);

        // Create Ghost instance
        var playerGhost = Instantiate(Ghost);
        playerGhost.GetComponent<SpriteRenderer>().color = player.GetComponent<SpriteRenderer>().color;
        playerGhost.GetComponent<TrailRenderer>().enabled = false;
        playerGhost.GetComponent<GhostBehaviour>().Forward = false;
        playerGhost.GetComponent<GhostBehaviour>().Path = WalkedPaths[RunNumber];
        playerGhost.transform.position = player.transform.position;
        ghosts.Add(playerGhost);

        // Upping the speed of the ghosts
        foreach (GameObject g in ghosts)
            g.GetComponent<GhostBehaviour>().PositionIndexIncrement = 150;
    }

    private void CheckDeathSequence()
    {
        // TODO: Please change this

        var ready = true;
        // Removing Ghosts when they're dead
        for (int i = ghosts.Count - 1; i >= 0; i--)
            if (!ghosts[i].GetComponent<GhostBehaviour>().WillDestroy)
                ready = false;


        if (ready)
        {
            // Resetting level
            ResetLevel();

            // Enabling player
            player.SetActive(true);
            player.GetComponent<Movement>().DeathCollision = false;

            isDead = false;

            DeadOverlay.GetComponent<DeadOverlay>().DisableDead();
        }
    }

    private void ResetLevel()
    {
        RunNumber = 0;
        KillAllGhosts();
        SpawnPlayer(StartPoints[RunNumber].position);
        for (int i = 0; i < StartPoints.Length; i++)
            WalkedPaths[i] = new List<Vector2>();
    }

    public void RemoveGhost(GameObject gameObject) => ghosts.Remove(gameObject);

    private void KillAllGhosts()
    {
        for (int i = ghosts.Count - 1; i >= 0; i--)
        {
            Destroy(ghosts[i]);
            ghosts.Remove(ghosts[i]);
        }
    }

    // Completing level
    private IEnumerator CompleteLevel()
    {
        // Change GameState
        State = States.Finished;
        // Destroy player
        Destroy(player);
        // Reset noise overlay to a opcatiy of 0
        SetMaterialOpacity(TVNoise, 0);
        
        // Fade out
        yield return StartCoroutine(FadeOutShader(TVNoise));
        yield return StartCoroutine(FadeOutSprite(WinFade));
    }

    private IEnumerator FadeOutSprite(GameObject gameObject)
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        var spriteColor = spriteRenderer.color;
        while(spriteColor.a < 1)
        {
            spriteColor.a += Increment;
            spriteRenderer.color = spriteColor;
            yield return null;
        }
    }

    private IEnumerator FadeOutShader(Material material)
    {
        var opacityName = material.shader.GetPropertyName(0);
        var opacity = material.GetFloat(opacityName);
        while (opacity < 1)
        {
            opacity += Increment;
            material.SetFloat(opacityName, opacity);
            yield return null;
        }
    }

    private void SetMaterialOpacity(Material material, float opacity) 
    {
        opacity = Mathf.Clamp(opacity, 0, 1);
        material.SetFloat(material.shader.GetPropertyName(0), opacity);
    }

}
