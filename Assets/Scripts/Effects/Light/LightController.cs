using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public float dayNightReference = 0;
    float totalLevels;
    public Light2D globalLight;
    //sun start location
    public GameObject sunObject;
    public Transform sunStartTransform;
    //sun end location
    public Transform sunEndTransform;
    public GameObject skyLightBlue;
    public GameObject skyOrange;
    public GameObject skyDarkBlue;
    public GameObject skyDefault;
    public GameObject foreGroundDay;
    public GameObject foreGroundNight;

    void Start()
    {
        totalLevels = GameManager.Instance.MaxLevel;
        CalculateDayNightReference();
        DecreaseGlobalLight();
        SkyChange();
        MoveSunObjectDown();
        SunSetLightIntensity();
    }

    void CalculateDayNightReference()
    {
        if (GameManager.Instance != null)
        {
            /*Debug.Log("Calculating day/night reference based on level percentage." + GameManager.Instance.levelPercentage);
            Debug.Log("LEVEL " + GameManager.Instance.GMLevel + "MAX LEVEL " + GameManager.Instance.MaxLevel + "one over the other" + GameManager.Instance.GMLevel / GameManager.Instance.MaxLevel);
            float progress = (float)GameManager.Instance.GMLevel / GameManager.Instance.MaxLevel;
            Debug.Log("progress" + progress);
            float chunkProgress = Mathf.Repeat(progress, 0.2f) / 0.2f;
            Debug.Log("chunkProgress" + chunkProgress);
            if (Mathf.Approximately(chunkProgress, 0f) || chunkProgress < 1e-4f)
                chunkProgress = 1f;

            dayNightReference = chunkProgress;
            Debug.Log("dayNightReference: " + dayNightReference);*/
            int subIndex = (GameManager.Instance.GMLevel - 1) % 20; // 0..19  ← this (%) is integer modulo
            float chunkProgress = (subIndex + 1) / 20f;             // 0.05..1.0 (level 20 maps to 1.0)

            dayNightReference = chunkProgress;
        }
        else
        {
            Debug.LogWarning("GameManager instance is not available.");
        }
        if (dayNightReference == 0)
        {
            dayNightReference = 1; 
        }
        else
        {
            Debug.Log("DAY NIGHT REFERENCE: " + dayNightReference);
        }
    }

    void DecreaseGlobalLight()
    {
        if (GameManager.Instance != null)
        {
            float lightIntensity = 1 - (dayNightReference);
            globalLight.intensity = lightIntensity;
            if (globalLight.intensity < 0.15f)
            {
                globalLight.intensity = 0.15f;
            }
        }
        else
        {
            Debug.LogWarning("GameManager instance is not available.");
        }
    }

    void MoveSunObjectDown()
    {
        sunObject.transform.position = Vector3.Lerp(sunStartTransform.position, sunEndTransform.position, dayNightReference);
    }
    void SkyChange()
    {
        if (dayNightReference >= 0.25f && dayNightReference < 0.50f)
        {
            Debug.Log("DAY NIGHT REFERENCE: " + dayNightReference   + " - Setting sky to light blue.");
            skyDefault.SetActive(false);
            skyLightBlue.SetActive(true);
            skyOrange.SetActive(false);
            skyDarkBlue.SetActive(false);
        }
        if (dayNightReference >= 0.50f && dayNightReference < 0.75f)
        {
            Debug.Log("DAY NIGHT REFERENCE: " + dayNightReference   + " - Setting sky to orange.");
            skyDefault.SetActive(false);
            skyLightBlue.SetActive(false);
            skyOrange.SetActive(true);
            skyDarkBlue.SetActive(false);
        }
        if (dayNightReference >= 0.75f && dayNightReference <= 1.0f)
        {
//            Debug.Log("DAY NIGHT REFERENCE: " + dayNightReference   + " - Setting sky to dark blue.");
            skyDefault.SetActive(false);
            skyLightBlue.SetActive(false);
            skyOrange.SetActive(false);
            skyDarkBlue.SetActive(true);
            foreGroundDay.SetActive(false);
            foreGroundNight.SetActive(true);
        }
    }

    void SunSetLightIntensity()
    {
        Light2D orangeLight = skyOrange.GetComponent<Light2D>();
        if (dayNightReference >= 0.5f && dayNightReference < 0.59f)
        {
            orangeLight.falloffIntensity = 0.6f;
        }
        if (dayNightReference >= 0.6f && dayNightReference < 0.69f)
        {
            orangeLight.falloffIntensity = 0.8f;
        }
        if (dayNightReference >= 0.7f && dayNightReference < 0.8f)
        {
            orangeLight.falloffIntensity = 1f;
        }
    }
}
