using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // Singleton pattern
    public static GameEvents current;

    private void Awake() => current = this;



    // Player touches something hazardous
    public event Action onPlayerDeathColission;

    public void OnPlayerDeathColission() => onPlayerDeathColission?.Invoke();



    // Player touches the finish
    public event Action onPlayerFinish;

    public void OnPlayerFinish() => onPlayerFinish?.Invoke();




    // Ghost gets destroyed
    public event Action<GameObject> onGhostDestroy;
    public void OnGhostDestroy(GameObject gameObject) => onGhostDestroy?.Invoke(gameObject);
}