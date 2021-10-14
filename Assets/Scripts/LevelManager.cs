using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Ghost;

    [Header("Start Points")]
    [SerializeField] private Transform[] StartPoints;

    [Header("Overlays")]
    [SerializeField] private GameObject DeadOverlay;
        
    [Header("Ghost Settings")]
    [SerializeField] private int GhostSpawnInterval = 2;

    [Header("Winning")]
    [SerializeField] private float Increment = .03f;

    // Playing states
    private enum States { Dead, Alive, Finished }
    private States State = States.Alive;


    private GameObject player;
    private int RunNumber = 0;
    private float StepInterval = .005f;

    // Ghost spawning
    private List<GameObject> ghosts = new List<GameObject>();
    float GhostSpawnTimer = 0;

    // Trail information
    private List<List<Vector2>> WalkedPaths = new List<List<Vector2>>();

    private void Start()
    {
        // Instantiate the list
        for (int i = 0; i < StartPoints.Length; i++)
            WalkedPaths.Add(new List<Vector2>());

        // Spawn the player in the level
        SpawnPlayer(StartPoints[RunNumber].position);
    }

    private void Update()
    {
        if(State == States.Alive)
        {
            if (Input.GetKeyDown(KeyCode.P)) StartCoroutine(CompleteLevel());
            SavePlayerPath();
            SpawnPathGhosts();
            CheckForPlayerColission();
        }
        if(State == States.Dead) CheckDeathSequence();
        if (State == States.Finished) return;
    }

    private void CheckForPlayerColission()
    {
        if (player.GetComponent<Movement>().DeathCollision)
            Dead();
    }


    // Previous path ghost
    private void SavePlayerPath()
    {
        if (WalkedPaths[RunNumber].Count < 1
            || Vector2.Distance(player.transform.position, WalkedPaths[RunNumber][WalkedPaths[RunNumber].Count - 1]) > StepInterval)
        {
            WalkedPaths[RunNumber].Add(player.transform.position);
        }
    }

    private void SpawnPathGhosts()
    {
        if (RunNumber > 0 && GhostSpawnTimer > GhostSpawnInterval)
        {
            // Reset Spawn Timer
            GhostSpawnTimer = 0;
            // Check how many ghost to spawn
            for (int i = 0; i < RunNumber; i++)
            {
                // Spawn ghosts and give them a path
                var ghost = Instantiate(Ghost);
                ghost.GetComponent<GhostBehaviour>().Path = WalkedPaths[i];
                ghost.GetComponent<GhostBehaviour>().Forward = false;
                ghost.GetComponent<GhostBehaviour>().levelManager = this;
                ghosts.Add(ghost);
            }
        }
        GhostSpawnTimer += Time.deltaTime;
    }


    // LevelManagement
    private void SpawnPlayer(Vector2 position)
    {
        if (player == null)
            player = Instantiate(Player) as GameObject;
        player.transform.position = position;
    }

    public void Finish()
    {
        RunNumber += 1;
        if (RunNumber < StartPoints.Length) SpawnPlayer(StartPoints[RunNumber].position);
        else StartCoroutine(CompleteLevel());
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
