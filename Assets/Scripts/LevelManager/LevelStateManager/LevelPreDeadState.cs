using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreDeadState : LevelBaseState
{
    LevelStateManager level;

    private void Start() => level = LevelStateManager.current;

    public override void EnterState() => StartCoroutine(DeathSequence());

    public override void LeaveState()
    {
    }

    public override void UpdateState()
    {
    }

    private void FreezeGhosts()
    {
        foreach (GameObject ghost in level.ghosts)
            ghost.GetComponent<GhostBehaviour>().Freeze();
    }

    private IEnumerator DeathSequence()
    {
        // Freeze all ghosts
        if (level.ghosts != null) FreezeGhosts();
        // Lock player in position
        level.player.GetComponent<Movement>().CanMove = false;
        level.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        level.player.GetComponent<Rigidbody2D>().simulated = false;
        // Show player death
        yield return LightManager.current.FocusOn(level.player.transform, level.TimeToFocusOn);
        yield return new WaitForSeconds(level.FocusWaitTime);
        yield return LightManager.current.FocusOff(level.TimeToFocusOff);
        // Allows player to be moved again
        level.player.GetComponent<Rigidbody2D>().simulated = true;
        level.player.GetComponent<Movement>().CanMove = true;
        // Switches state
        level.SwitchState(level.DyingState);
    }
}
