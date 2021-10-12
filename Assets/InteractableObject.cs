using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector3 offset;

    public void Update()
    {
        canvas.gameObject.transform.position = transform.position + offset;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Enable showing E button to enter portal
        if(collision.gameObject.tag == PlayerTag)
            canvas.GetComponent<UIButton>().Show();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
            canvas.GetComponent<UIButton>().Hide();
    }
}