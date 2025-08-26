using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StompActivator : MonoBehaviour
{
    public Image blackImage;
    public float fadeDuration = 10f;
    public GameObject stompGuyAndBorders;
    public GameObject currentBorders;
    public GameObject spawnPoint;
    public GameObject endScenario;
    public bool bossLevel = false;
    //if level = 10 activate stomp guy and deactivate borders

    void Awake()
    {
        StartCoroutine(StompActivatorMethod());
    } 
    void Start()
    {
        
    }
    IEnumerator StompActivatorMethod()
    {
        yield return new WaitForEndOfFrame(); // Ensure the game is fully initialized before checking level percentage\
        LightController lightController = FindObjectOfType<LightController>();
        if (GameManager.Instance.GMLevel == 20 || GameManager.Instance.GMLevel == 40 || GameManager.Instance.GMLevel == 60 || GameManager.Instance.GMLevel == 80 || GameManager.Instance.GMLevel == 100 || GameManager.Instance.GMLevel == 120|| GameManager.Instance.GMLevel == 140|| GameManager.Instance.GMLevel == 160|| GameManager.Instance.GMLevel == 180|| GameManager.Instance.GMLevel == 200)
        {
            bossLevel = true;
            AudioManager.Instance.PlaySFX(SFXType.bossAppears, 1f);
            Time.timeScale = 0f; // Pause the game
            spawnPoint.transform.position = new Vector2(0, 3.77f);
            endScenario.transform.position = new Vector2(0, -1.61f);
            yield return StartCoroutine(FadeToBlack());
            stompGuyAndBorders.SetActive(true);
            currentBorders.SetActive(false);
            yield return StartCoroutine(FadeFromBlack());
            Time.timeScale = 1f; // Resume the game
        }

    }
    

    public IEnumerator FadeToBlack()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackImage.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeFromBlack()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackImage.color = new Color(0, 0, 0, 0);
    }

    // Optional helper to do both
    public IEnumerator FadeToBlackThenBack(float delay = 0.5f)
    {
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeFromBlack());
    }
}

