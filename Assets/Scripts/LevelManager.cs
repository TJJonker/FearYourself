using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Ghost;

    [Header("Start Points")]
    [SerializeField] private Transform[] StartPoints;

    [Header("Overlays")]
    [SerializeField] private GameObject DeadOverlay;
    [SerializeField] private GameObject WinFade;
    
    [Header("Ghost Settings")]
    [SerializeField] private int GhostSpawnInterval = 2;

    private GameObject player;
    private int RunNumber = 0;

    private float StepInterval = .005f;

    private bool isDead = false;


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
        if (Input.GetKeyDown(KeyCode.P)) Dead();

        // TODO: Cleanup this messy shit
        if (isDead) CheckDeathSequence();

        if (isDead) return;
        SavePlayerPath();
        SpawnPathGhosts();
        CheckForPlayerColission();
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
        if (RunNumber < StartPoints.Length)
        {
            SpawnPlayer(StartPoints[RunNumber].position);
        }
        else
        {
            ResetLevel();
        }
    }

    public void Dead()
    {
        // Enable Dead overlay
        DeadOverlay.GetComponent<DeadOverlay>().EnableDead();

        // Set LevelManager to Dead
        isDead = true;

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

    public void RemoveGhost(GameObject gameObject)
    {
        ghosts.Remove(gameObject);
    }

    private void KillAllGhosts()
    {
        for (int i = ghosts.Count - 1; i >= 0; i--)
        {
            Destroy(ghosts[i]);
            ghosts.Remove(ghosts[i]);
        }
    }
}
