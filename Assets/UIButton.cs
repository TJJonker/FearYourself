using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject frontButton;
    [SerializeField] private GameObject backButton;

    private Image frontImage;
    private Image backImage;

    private Color frontImageColor;
    private Color backImageColor;

    [SerializeField] private float maxAlpha;
    [SerializeField] private float alphaIncrement;

    private void Start()
    {
        frontImage = frontButton.GetComponent<Image>();
        Hide();
    }

    private IEnumerator Appear()
    {
        frontImageColor = frontImage.color;
        if (frontImageColor.a < maxAlpha)
        {
            frontImageColor.a += alphaIncrement;
            frontImage.color = frontImageColor;
            yield return null;
        }
        else
        {
            frontImageColor.a = maxAlpha;
            frontImage.color = frontImageColor;
        }
    }

    public void Hide()
    {
        frontButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    public void Show() => StartCoroutine(Shw());

    public IEnumerator Shw()
    {
        Debug.Log("Started");
        yield return StartCoroutine(Appear());
        Debug.Log("Stopped");
    }
}