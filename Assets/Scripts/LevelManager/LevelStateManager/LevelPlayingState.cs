using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayingState : LevelBaseState
{
    // Trail Information
    private const float STEP_INTERVAL = .005f;

    // Reference to LevelStateManager
    LevelStateManager level;


    public override void EnterState()
    {
        level = LevelStateManager.current;
        // Prepare the WalkedPaths array
        InstantiateWalkedPaths();
        // Spawn the player in the level
        level.SpawnPlayer(level.StartPoints[level.RunNumber].position);
        StartCoroutine(SpawnPathGhosts());

        // Subscribe to event system triggers
        GameEvents.current.onPlayerDeathColission += Dead;
        GameEvents.current.onPlayerFinish += Finish;
    }

    public override void UpdateState()
    {
        // Saving player-walked path
        SavePlayerPath();

        if (Input.GetKeyDown(KeyCode.P)) Dead();
        if (Input.GetKeyDown(KeyCode.O)) Winning();

        // Increase run timer
        level.RunTimer += Time.deltaTime;
    }

    public override void LeaveState()
    {
        // What needs to happen before leaving the state
        GameEvents.current.onPlayerDeathColission -= Dead;
        GameEvents.current.onPlayerFinish -= Finish;
        StopAllCoroutines();
    }



    private void InstantiateWalkedPaths()
    {
        // Check whether the list is long enough or not
        if (level.WalkedPaths != null && level.WalkedPaths.Count >= level.StartPoints.Length) return;
        // Instantiate the list
        level.WalkedPaths = new List<List<Vector2>>();
        for (int i = 0; i < level.StartPoints.Length; i++)
            level.WalkedPaths.Add(new List<Vector2>());
    }

    // Previous path ghost
    private void SavePlayerPath()
    {
        var playerPosition = level.player.transform.position;
        // First step or Certain distance
        if (level.WalkedPaths[level.RunNumber].Count < 1 
            || Vector2.Distance(playerPosition, level.WalkedPaths[level.RunNumber][level.WalkedPaths[level.RunNumber].Count - 1]) > STEP_INTERVAL)
            level.WalkedPaths[level.RunNumber].Add(level.player.transform.position);
    }

    private IEnumerator SpawnPathGhosts()
    {
        while (true)
        {
            for (int i = 0; i < level.RunNumber; i++)
                SpawnGhost(i);
            yield return new WaitForSeconds(level.GhostSpawnInterval);
        }
    }

    // Spawn an X amount of ghosts
    private void SpawnGhost(int runNumber)
    {
        var ghost = Instantiate(level.Ghost);
        var ghostBehaviour = ghost.GetComponent<GhostBehaviour>();
        ghostBehaviour.Path = level.WalkedPaths[runNumber];
        ghostBehaviour.forward = false;
        ghostBehaviour.speed = level.GhostSpeed;
        level.ghosts.Add(ghost);
    }

    private void Finish()
    {
        level.RunNumber += 1;
        if (level.RunNumber < level.StartPoints.Length)
            level.SpawnPlayer(level.StartPoints[level.RunNumber].position);
        else level.SwitchState(level.WinningState);
    }

    private void Dead() => level.SwitchState(level.PreDyingState);
    private void Winning() => level.SwitchState(level.WinningState);
}