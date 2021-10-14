using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBaseState : MonoBehaviour
{
    public abstract void EnterState(LevelStateManager level);
    public abstract void UpdateState(LevelStateManager level);
}
