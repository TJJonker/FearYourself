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

    [Range(0, 1)] [SerializeField] private float maxAlpha;
    [SerializeField] private float alphaIncrement;

    private void Start()
    {
        frontImage = frontButton.GetComponent<Image>();
        Hide();
    }

    private IEnumerator Disappear()
    {
        frontImageColor = frontImage.color;
        while (frontImageColor.a > 0)
        {
            frontImageColor.a -= alphaIncrement;
            frontImage.color = frontImageColor;
            yield return null;
        }
    }

    private IEnumerator Appear()
    {
        frontImageColor = frontImage.color;
        while (frontImageColor.a < maxAlpha)
        {
            frontImageColor.a += alphaIncrement;
            frontImage.color = frontImageColor;
            yield return null;
        }
    }

    public void Hide() => Disappearing = StartCoroutine(Hde());
    
    // TODO: Add a cache and null check
    public void Show() => Showing = StartCoroutine(Shw());
   
    public IEnumerator Hde()
    {
        yield return StartCoroutine(Disappear());
    }

    public IEnumerator Shw()
    {
        yield return StartCoroutine(Appear());
    }
    
}