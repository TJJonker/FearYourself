using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class LevelCompleted : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI LevelCompletedText;
    [SerializeField] private TextMeshProUGUI Time1stStar;
    [SerializeField] private TextMeshProUGUI Time2ndStar;
    [SerializeField] private TextMeshProUGUI Time3rdStar;
    [SerializeField] private TextMeshProUGUI PlayerTime;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TextMeshProUGUI ClickToContinue;

    [Header("Stars")]
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;

    private Coroutine levelCompleteSequence;

    void Awake() => HideAll();

    private void HideAll()
    {
        LevelCompletedText.alpha = 0;
        Time1stStar.alpha = 0;
        Time2ndStar.alpha = 0;
        Time3rdStar.alpha = 0;
        PlayerTime.alpha = 0;
        TimeText.alpha = 0;
        ClickToContinue.alpha = 0;
        ClickToContinue.GetComponent<BlinkEffect>().enabled = false;
        star1.color = new Color(star1.color.r, star1.color.g, star1.color.b, 0);
        star2.color = new Color(star1.color.r, star1.color.g, star1.color.b, 0);
        star3.color = new Color(star1.color.r, star1.color.g, star1.color.b, 0);
    }

    public void StartLevelCompleteSequence(float playerTime, float starTime1, float starTime2, float starTime3)
    {
        // Set values
        SetTimeValue(Time1stStar, starTime1);
        SetTimeValue(Time2ndStar, starTime2);
        SetTimeValue(Time3rdStar, starTime3);

        if (playerTime < starTime1) SetTimeValue(PlayerTime, starTime1 + 15);
        else SetTimeValue(PlayerTime, playerTime);

        // Start Coroutine
        if (levelCompleteSequence != null) StopCoroutine(levelCompleteSequence);
        levelCompleteSequence = StartCoroutine(LevelCompleteSequence(playerTime, starTime1, starTime2, starTime3));
    }

    public IEnumerator LevelCompleteSequence(float playerTime, float starTime1, float starTime2, float starTime3)
    {
        // Phase 1: Show LevelCompleted text
        yield return Appear(LevelCompletedText, 2);

        // Phase 2: Show stars and information
        yield return Appear(star1, 2);
        yield return Appear(Time1stStar, 2);
        yield return Appear(star2, 2);
        yield return Appear(Time2ndStar, 2);
        yield return Appear(star3, 2);
        yield return Appear(Time3rdStar, 2);

        // Phase 3: Show time and maxed Time
        yield return Appear(TimeText, 2);
        yield return Appear(PlayerTime, 2);

        // Phase 4: countdown if playerTime is faster than slowest star-time
        if (playerTime < starTime1) yield return StarSequence(playerTime, starTime1, starTime2, starTime3);
        yield return new WaitForSeconds(1);
        ClickToContinue.GetComponent<BlinkEffect>().enabled = true;
    }

    private IEnumerator StarSequence(float playerTime, float starTime1, float starTime2, float starTime3)
    {
        bool _star1 = false, _star2 = false, _star3 = false;
        var time = starTime1 + 15;
        while(time > playerTime)
        {
            time -= 25 * Time.deltaTime;
            if (time <= starTime1 && !_star1)
            {
                _star1 = true;
                time = starTime1;
                SetTimeValue(PlayerTime, time);
                yield return StarEffect(star1, .5f);
            }
            if (time <= starTime2 && !_star2)
            {
                _star2 = true;
                time = starTime2;
                SetTimeValue(PlayerTime, time);
                yield return StarEffect(star2, .5f);
            }
            if (time <= starTime3 && !_star3)
            {
                _star3 = true;
                time = starTime3;
                SetTimeValue(PlayerTime, time);
                yield return StarEffect(star3, .5f, true);
            }
            SetTimeValue(PlayerTime, time);
            yield return null;
        }
    }

    private IEnumerator Appear(TextMeshProUGUI text, float speed, float maxOpacity = 1)
    {
        var textColor = text.color;
        while(textColor.a < maxOpacity)
        {
            textColor.a += speed * Time.deltaTime;
            text.color = textColor;
            yield return null;
        }
    }
    
    private IEnumerator Appear(Image text, float speed, float maxOpacity = 1)
    {
        var textColor = text.color;
        while(textColor.a < maxOpacity)
        {
            textColor.a += speed * Time.deltaTime;
            text.color = textColor;
            yield return null;
        }
    }

    private void SetTimeValue(TextMeshProUGUI text, float seconds)
    {
        text.text = $"{(int)seconds / 60}:{seconds % 60:00.000}";
    }

    private IEnumerator StarEffect(Image star, float time, bool particleEffect = false)
    {
        var colorIncrement = (new Color(222, 200, 91)/255 - star.color/255) / time;
        float t = 0;
        while (t < time)
        {
            star.color += colorIncrement * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        if (particleEffect) Debug.Log("Whew");
    }
}
