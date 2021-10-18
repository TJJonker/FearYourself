using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager current;

    // TODO: Save start intensity to return to original intensity instead of 1 or 0

    [SerializeField] private GameObject globalLight;
    [SerializeField] private GameObject spotlight;

    private Coroutine focus;

    private void Awake() => current = this;

    public Coroutine FocusOn(Transform pos, float speed, float darkScale = .25f)
    {
        if (focus != null) StopCoroutine(focus);
        return focus = StartCoroutine(focusOn(pos, speed, darkScale));
    }

    public Coroutine FocusOff(float speed)
    {
        if (focus != null) StopCoroutine(focus);
        return focus = StartCoroutine(focusOff(speed));
    }

    // TODO: Make this modular: Make one function that checks whether the intensity is higher or lower
    // and let that function call the Coroutines. 

    private IEnumerator focusOn(Transform pos, float timeToFinish, float darkScale = .25f)
    {
        spotlight.transform.position = pos.position;
        // Collect light data
        var globalLightScript = globalLight.GetComponent<Light2D>();
        var spotlightScript = spotlight.GetComponent<Light2D>();
        // Calculate Scale variables
        darkScale = Mathf.Clamp(darkScale, 0, 1);
        var speed = (1 - darkScale) / timeToFinish;

        // Run script
        for (float i = 0; i < timeToFinish; i += Time.deltaTime)
        {
            globalLightScript.intensity -= speed * Time.deltaTime;
            spotlightScript.intensity += speed * Time.deltaTime;
            yield return null;
        }
        // Setting 
        globalLightScript.intensity = darkScale;
        spotlightScript.intensity = 1 - darkScale;
    }

    private IEnumerator focusOff(float timeToFinish)
    {
        // Collect light data
        var globalLightScript = globalLight.GetComponent<Light2D>();
        var spotlightScript = spotlight.GetComponent<Light2D>();
        // Calculate Scale variables
        var darkScale = globalLightScript.intensity;
        var speed = (1 - darkScale) / timeToFinish;
        // Run script
        for(float i = 0; i < timeToFinish; i += Time.deltaTime)
        {
            globalLightScript.intensity += speed * Time.deltaTime;
            spotlightScript.intensity -= speed * Time.deltaTime;
            yield return null;
        }
        globalLightScript.intensity = 1;
        spotlightScript.intensity = 0;
    }

}
