using System.Collections.Generic;
using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    // Singleton pattern
    public static LevelStateManager current;

    // Current state
    private LevelBaseState currentState;

    // Finite states
    [Header("Level States")]
    [SerializeField] private GameObject StatePlaying;

    [System.NonSerialized] public LevelPlayingState PlayingState;
    [System.NonSerialized] public LevelWinningState WinningState;
    [System.NonSerialized] public LevelDyingState DyingState;

    [Header("Prefabs")]
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Ghost;

    [Header("Start Points")]
    [SerializeField] public Transform[] StartPoints;

    [Header("Ghost Settings")]
    [SerializeField] public int GhostSpawnInterval = 2;
    [SerializeField] public int GhostSpeed = 50;

    [Header("FadeSettings")]
    [SerializeField] public float FadeOutSpeed = 0.03f;
    [SerializeField] public float FadeInSpeed = 0.03f;

    // Private Ghost settings
    [System.NonSerialized] public List<GameObject> ghosts = new List<GameObject>();

    // Private Player settings
    [System.NonSerialized] public GameObject player;

    // Private Level settings
    [System.NonSerialized] public List<List<Vector2>> WalkedPaths;
    [System.NonSerialized] public int RunNumber;


    private void Awake() => current = this;

    private void Start()
    {
        PlayingState = StatePlaying.GetComponent<LevelPlayingState>();
        WinningState = StatePlaying.GetComponent<LevelWinningState>();
        DyingState = StatePlaying.GetComponent<LevelDyingState>();

        GameEvents.current.onGhostDestroy += RemoveGhost;
        SwitchState(PlayingState);
    }

    private void Update() => currentState.UpdateState();

    public void SwitchState(LevelBaseState state)
    {
        if(currentState) currentState.LeaveState();
        currentState = state;
        currentState.EnterState();
        Debug.Log(currentState);
    }
    public void SpawnPlayer(Vector2 position)
    {
        // Spawn a player if there is none, otherwise move to position
        if (player == null) player = Instantiate(Player);
        // TODO: Change to remove trailrenderer when moved and make it appear afterwards.
        player.transform.position = position;
    }
    public void RemoveGhost(GameObject gameObject) 
        => ghosts.Remove(gameObject);
}