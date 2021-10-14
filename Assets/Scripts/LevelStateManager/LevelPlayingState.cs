using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayingState : LevelBaseState
{
    // Trail information
    private List<List<Vector2>> WalkedPaths;
    private const float STEP_INTERVAL = .005f;
    private int RunNumber;

    // player variables
    private GameObject player;
    private PlayerManager playerManager;


    public override void EnterState(LevelStateManager level)
    {
        // Prepare the WalkedPaths array
        InstantiateWalkedPaths(level);
        // Spawn the player in the level
        SpawnPlayer(level, level.StartPoints[RunNumber].position);
        StartCoroutine(SpawnPathGhosts(level));
    }

    public override void UpdateState(LevelStateManager level)
    {
        // Saving player-walked path
        SavePlayerPath();
        CheckForFinish(level);
    }

    private void SpawnPlayer(LevelStateManager level, Vector2 position)
    {
        // Spawn a player if there is none, otherwise move to position
        if (player == null)
        {
            player = Instantiate(level.Player);
            playerManager = player.GetComponent<PlayerManager>();
        }
        // TODO: Change to remove trailrenderer when moved and make it appear afterwards.
        player.transform.position = position;
    }

    private void InstantiateWalkedPaths(LevelStateManager level)
    {
        // Check whether the list is long enough or not
        if (WalkedPaths.Count >= level.StartPoints.Length) return;
        // Instantiate the list
        WalkedPaths = new List<List<Vector2>>();
        for (int i = 0; i < level.StartPoints.Length; i++)
            WalkedPaths.Add(new List<Vector2>());
    }

    // Previous path ghost
    private void SavePlayerPath()
    {
        var playerPosition = player.transform.position;
        var lastStepPosition = WalkedPaths[RunNumber][WalkedPaths[RunNumber].Count - 1];
        // First step or Certain distance
        if (WalkedPaths[RunNumber].Count < 1 || Vector2.Distance(playerPosition, lastStepPosition) > STEP_INTERVAL)
            WalkedPaths[RunNumber].Add(player.transform.position);
    }

    // TODO: Add code when player is hit => Dead()

    private IEnumerator SpawnPathGhosts(LevelStateManager level)
    {
        while (RunNumber > 0)
        {
            for (int i = 0; i < RunNumber; i++)
                SpawnGhost(level, RunNumber);
            yield return new WaitForSeconds(level.GhostSpawnInterval);
        }
    }

    // Spawn an X amount of ghosts
    private void SpawnGhost(LevelStateManager level, int runNumber)
    {
        var ghost = Instantiate(level.Ghost);
        ghost.GetComponent<GhostBehaviour>().Path = WalkedPaths[runNumber];
        ghost.GetComponent<GhostBehaviour>().Forward = false;
        ghost.GetComponent<GhostBehaviour>().levelManager = this;
        ghosts.Add(ghost);
    }


    // TODO: Maybe add a listening Pattern?
    public void CheckForFinish(LevelStateManager level)
    {
        // Check whether the player touches the finish
        if (!playerManager.CheckForFinish()) return;

        RunNumber += 1;
        if (RunNumber < level.StartPoints.Length) 
            SpawnPlayer(level, level.StartPoints[RunNumber].position);
        else level.SwitchState(level.WinningState);
    }
}