using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSpawnLocation : MonoBehaviour
{
    Vector2 rayOrigin;
    Vector2 rayDirection;
    float rayDistance;
    public float rayXAdjustment = 0.5f;
    public float rayYAdjustment = 0.5f;
    public LetterSpawnScript letterSpawnScript;
    public GameObject endScenarioText;
    float requiredTime = 10f;
    public float timer = 0f;

    void Update()
    {
        RayCastToEndScenario();

    }
    void Start()
    {
        if (letterSpawnScript == null)
        {
            letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
            if (letterSpawnScript == null)
            {
                Debug.LogError("LetterSpawnScript not found in the scene.");
                return;
            }
        }

    }

    void RayCastToEndScenario()
    {
        rayOrigin = new Vector2(transform.position.x - rayXAdjustment, transform.position.y - rayYAdjustment);
        rayDirection = Vector2.right;
        rayDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("LetterOnGrid"))
        {
            if (letterSpawnScript != null)
            {
                float xValue = transform.position.x;
                letterSpawnScript.RemoveFromXSpawnValues(xValue);
                timer += Time.deltaTime;
                if (timer >= requiredTime)
                {
                    timer = 0f;
                    Debug.Log($"[{name}] Under ray for {timer:F2} seconds, triggering end scenario.");
                    StartCoroutine(EndScenarioCoroutine());
                }
            }
        }
        else
        {
            timer = 0f;
            if (letterSpawnScript != null)
            {
                float xValue = transform.position.x;
                //if xvalue is not on the removeXValues list
                if (letterSpawnScript.removeXValues.Contains(xValue))
                {
                    //Debug.Log($"x value {xValue} is not in removeXValues list, adding it back.");
                    letterSpawnScript.AddToXSpawnValues(xValue);
                    //Debug.Log($"Added x value {xValue} back to LetterSpawnScript.");
                }

            }
        }
    }

    IEnumerator EndScenarioCoroutine()
    {
        TimerForEndGame timerForEndGame = FindObjectOfType<TimerForEndGame>();
        if (timerForEndGame != null)
        {
            Destroy(timerForEndGame.gameObject);
        }
        endScenarioText.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


