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
        yield return new WaitForEndOfFrame(); // Ensure the game is fully initialized before checking level percentage
        if (GameManager.Instance.levelPercentage == 0.2f || GameManager.Instance.levelPercentage == 0.4f || GameManager.Instance.levelPercentage == 0.6f || GameManager.Instance.levelPercentage == 0.8f || GameManager.Instance.levelPercentage == 1.0f)
        {
            Debug.Log("Activating stomp guy and deactivating borders for level: " + GameManager.Instance.levelPercentage);
            Time.timeScale = 0f; // Pause the game
            spawnPoint.transform.position = new Vector2(0, 3.77f);
            endScenario.transform.position = new Vector2(0, 2.67f);
            Debug.Log("Starting stomp activation coroutine.");
            yield return StartCoroutine(FadeToBlack());
            Debug.Log("Fading to black for stomp activation.");
            stompGuyAndBorders.SetActive(true);
            currentBorders.SetActive(false);
            yield return StartCoroutine(FadeFromBlack());
            Debug.Log("Fading from black for stomp activation.");
            Time.timeScale = 1f; // Resume the game
            Debug.Log("UNPAUSE.");
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

