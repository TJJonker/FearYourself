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
        level.player.SetActive(false);
        Overlay.current.SetOpacity(Overlay.Overlays.TVStatic, 0);
        yield return Overlay.current.StartFadeOut(Overlay.Overlays.TVStatic, level.FadeOutSpeed);
        yield return Overlay.current.StartFadeOut(Overlay.Overlays.WinFade, level.FadeOutSpeed);
    }


}
