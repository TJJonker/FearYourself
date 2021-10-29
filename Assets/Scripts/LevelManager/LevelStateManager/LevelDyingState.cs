using System.Collections.Generic;
using UnityEngine;

public class LevelDyingState : LevelBaseState
{
    private LevelStateManager level;

    private const int DEAD_GHOST_SPEED = 150;

    public override void EnterState()
    {
        level = LevelStateManager.current;

        level.player.SetActive(false);
        EnableDeadOverlay();
        ReverseGhostPath();
        CreatePlayerGhost();
        ChangeGhostSpeed(DEAD_GHOST_SPEED);

        // Reset RunTimer
        level.RunTimer = 0;
    }

    public override void UpdateState()
    {
        DeathSequence();
    }

    public override void LeaveState()
    {
    }

    private void DeathSequence()
    {
        if (level.ghosts.Count > 0) return;
        ResetLevel();
        level.player.SetActive(true);
        DisableDeadOverlay();
        level.SwitchState(level.PlayingState);
    }

    // Helper methods
    private void EnableDeadOverlay()
    {
        Overlay.current.SetOpacity(Overlay.Overlays.TVStatic, 1);
        Overlay.current.SetOpacity(Overlay.Overlays.Rewind, 1);
    }

    private void DisableDeadOverlay()
    {
        Overlay.current.SetOpacity(Overlay.Overlays.TVStatic, 0);
        Overlay.current.SetOpacity(Overlay.Overlays.Rewind, 0);
    }

    private void ReverseGhostPath()
    {
        foreach (GameObject g in level.ghosts)
            g.GetComponent<GhostBehaviour>().forward = true;
    }

    private void CreatePlayerGhost()
    {
        var playerGhost = Instantiate(level.Ghost);
        playerGhost.GetComponent<SpriteRenderer>().color = level.player.GetComponent<SpriteRenderer>().color;
        playerGhost.GetComponent<TrailRenderer>().enabled = false;
        playerGhost.GetComponent<GhostBehaviour>().forward = false;
        playerGhost.GetComponent<GhostBehaviour>().Path = level.WalkedPaths[level.RunNumber];
        playerGhost.transform.position = level.player.transform.position;
        level.ghosts.Add(playerGhost);
    }

    private void ChangeGhostSpeed(int speed)
    {
        foreach (GameObject g in level.ghosts)
            g.GetComponent<GhostBehaviour>().speed = speed;
    }

    private void ResetLevel()
    {
        // Reset runnumber
        level.RunNumber = 0;
        // Respawn player at first startpoint
        level.SpawnPlayer(level.StartPoints[level.RunNumber].position);
        // Reset Path lists
        for (int i = 0; i < level.WalkedPaths.Count; i++)
            level.WalkedPaths[i] = new List<Vector2>();
    }
}