using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public enum Overlays { TVStatic, Rewind, WinFade }
    private Dictionary<Overlays, GameObject> overlays;

    [Header("Overlays")]
    [SerializeField] private GameObject TVStatic;
    [SerializeField] private GameObject Rewind;
    [SerializeField] private GameObject WinFade;

    private void Start()
    {
        overlays.Add(Overlays.TVStatic, TVStatic);
        overlays.Add(Overlays.Rewind, Rewind);
        overlays.Add(Overlays.WinFade, WinFade);
    }

    public void SetOpacity(Overlays overlay, float opacity)
    {
        GameObject gameObject;
        overlays.TryGetValue(overlay, out gameObject);
        var material = gameObject.GetComponent<Material>();
        material.SetFloat(material.shader.GetPropertyName(0), opacity);
    }
}
