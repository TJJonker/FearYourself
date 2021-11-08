using System.Collections;
using UnityEngine;

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
