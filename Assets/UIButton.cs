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
        while (frontImageColor.a < maxAlpha)
        {
            frontImageColor.a += alphaIncrement;
            frontImage.color = frontImageColor;
            Debug.Log(frontImageColor.a);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Hide()
    {
        frontButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
    
    // TODO: Add a cache and null check
    public void Show() => StartCoroutine(Shw());

    public IEnumerator Shw()
    {
        Debug.Log("Started");
        yield return StartCoroutine(Appear());
        Debug.Log("Stopped");
    }
}