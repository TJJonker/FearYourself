using UnityEngine;

public class LevelSelectorState : LevelBaseState
{
    LevelStateManager level;

    public override void EnterState()
    {
        level = LevelStateManager.current;
        // Spawn the player in the level
        level.SpawnPlayer(level.StartPoints[level.RunNumber].position);
    }

    public override void LeaveState()
    {
    }

    public override void UpdateState()
    {
    }
}
