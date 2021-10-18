using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public static Overlay current;

    [Header("ShaderGraph Overlays")]
    [SerializeField] private GameObject TVStatic;
    [SerializeField] private GameObject Rewind;
    [SerializeField] private GameObject WinFade;

    public enum Overlays { TVStatic, Rewind, WinFade }
    private Dictionary<Overlays, GameObject> overlays = new Dictionary<Overlays, GameObject>();

    private Coroutine[] routines;

    private void Awake() => current = this;

    private void Start()
    {
        overlays.Add(Overlays.TVStatic, TVStatic);
        overlays.Add(Overlays.Rewind, Rewind);
        overlays.Add(Overlays.WinFade, WinFade);

        routines = new Coroutine[overlays.Count];
    }

    private Material OverlayToMaterial(Overlays overlay)
    {
        GameObject gameObject;
        overlays.TryGetValue(overlay, out gameObject);
        return gameObject.GetComponent<SpriteRenderer>().material;
    }

    public void SetOpacity(Material material, float opacity) 
        => material.SetFloat(material.shader.GetPropertyName(0), opacity);

    public void SetOpacity(Overlays overlay, float opacity)
    {
        var material = OverlayToMaterial(overlay);
        material.SetFloat(material.shader.GetPropertyName(0), opacity);
    }

    public Coroutine StartFadeOut(Overlays overlay, float increment, float maxOpacity = 1)
    {
        if (routines[(int)overlay] != null) StopCoroutine(routines[(int)overlay]);
        return StartCoroutine(FadeOut(overlay, increment, maxOpacity));
    }

    public Coroutine StartFadeIn(Overlays overlay, float increment, float minOpacity = 0)
    {
        if (routines[(int)overlay] != null) StopCoroutine(routines[(int)overlay]);
        return StartCoroutine(FadeIn(overlay, increment, minOpacity));
    }

    private IEnumerator FadeOut(Overlays overlay, float increment, float maxOpacity = 1)
    {
        var material = OverlayToMaterial(overlay);
        maxOpacity = Mathf.Clamp(maxOpacity, 0, 1);
        var opacity = material.GetFloat(material.shader.GetPropertyName(0));
        while(opacity < maxOpacity)
        {
            opacity += increment * Time.deltaTime;
            SetOpacity(material, opacity);
            yield return null;
        }
        SetOpacity(material, maxOpacity);
    }    
    
    private IEnumerator FadeIn(Overlays overlay, float increment, float minOpacity = 0)
    {
        var material = OverlayToMaterial(overlay);
        minOpacity = Mathf.Clamp(minOpacity, 0, 1);
        var opacity = material.GetFloat(material.shader.GetPropertyName(0));
        while(opacity > minOpacity)
        {
            opacity -= increment * Time.deltaTime;
            SetOpacity(material, opacity);
            yield return null;
        }
        SetOpacity(material, minOpacity);
    }
}
