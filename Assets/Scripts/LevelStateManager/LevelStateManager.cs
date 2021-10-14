using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    // Current state
    LevelBaseState currentState;

    // Finite states
    [System.NonSerialized] public LevelPlayingState PlayingState = new LevelPlayingState();
    [System.NonSerialized] public LevelDyingState DyingState = new LevelDyingState();
    [System.NonSerialized] public LevelWinningState WinningState = new LevelWinningState();

    [Header("Prefabs")]
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Ghost;

    [Header("Start Points")]
    [SerializeField] public Transform[] StartPoints;

    [Header("Ghost Settings")]
    [SerializeField] public int GhostSpawnInterval = 2;



    private void Start() => SwitchState(PlayingState);

    private void Update() => currentState.UpdateState(this);

    public void SwitchState(LevelBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
