using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LetterLight : MonoBehaviour
{
    public float dayNightReference = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator TurnOffLights()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance != null)
        {
            float progress = (float)GameManager.Instance.GMLevel / 40;
            float chunkProgress = (progress % 0.25f) / 0.25f;
            dayNightReference = chunkProgress;
            Light2D light = GetComponent<Light2D>();

            Debug.Log("light increased to " + light.intensity + " for " + gameObject.name);
            yield return new WaitForSeconds(1f);
            light.intensity = 10f;
            Debug.Log("light increased to " + light.intensity + " for " + gameObject.name);
            yield return new WaitForSeconds(1f);
            Debug.Log("light increased to " + light.intensity + " for " + gameObject.name);
        }
    }
}
