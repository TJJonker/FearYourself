using System.Collections;
using UnityEngine;

public struct UIObject 
{
    private Texture2D image;
    private Texture2D backgroundImage;

    public UIObject(Texture2D image, Texture2D backgroundImage, Vector2 size)
    {
        this.image = image;
        this.backgroundImage = backgroundImage;
        image.Resize((int)size.x, (int)size.y);
    }

    public void Activate()
    {

    }

    public IEnumerator Disappear()
    {

        return null;
    }
}
