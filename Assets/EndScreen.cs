using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown) SceneManager.LoadScene(1);
    }
}