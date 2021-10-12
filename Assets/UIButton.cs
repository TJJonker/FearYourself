using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject frontButton;
    [SerializeField] private GameObject backButton;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        frontButton.GetComponent<Image>().color = new Color(255, 255, 255, 100);
    }

    public void Hide()
    {
        frontButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
}
