using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinningState : LevelBaseState
{
    LevelStateManager level;


    private void Start() => level = LevelStateManager.current;

    public override void EnterState()
    {
        StartCoroutine(CompleteLevel());
    }

    public override void LeaveState()
    {
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        if (Input.anyKeyDown) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator CompleteLevel()
    {
        yield return FadeOut();
        level.WinningStateGUI.StartLevelCompleteSequence(level.RunTimer, 
            level.SecondsFirstStar, level.SecondsSecondStar, level.SecondsThirdStar);
    }

    private IEnumerator FadeOut()
    {
        level.player.SetActive(false);
        Overlay.current.SetOpacity(Overlay.Overlays.TVStatic, 0);
        yield return Overlay.current.StartFadeOut(Overlay.Overlays.WinFade, level.FadeOutSpeed);
        //yield return Overlay.current.StartFadeOut(Overlay.Overlays.TVStatic, level.FadeOutSpeed);
    }
}
