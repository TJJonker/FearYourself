using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private const string TAG_OBSTACLE = "Obstacle";
    private const string TAG_FINISH = "Finish";

    // Checks for collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_OBSTACLE) GameEvents.current.OnPlayerDeathColission();
        if (collision.gameObject.tag == TAG_FINISH) GameEvents.current.OnPlayerFinish();
    }
}
