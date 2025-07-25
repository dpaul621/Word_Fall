using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreAddedImage : MonoBehaviour
{
    public TextMeshProUGUI thisScoreTextMessage;
    public float fadeDuration = 2f;
    void Start()
    {
        
    }
    public void ShowScoreAddedMessage()
    {
        //enable this score text component
        StartCoroutine(FadeOutAndDisable(thisScoreTextMessage));
    }
    private IEnumerator FadeOutAndDisable(TextMeshProUGUI tmpText)
    {
        tmpText.gameObject.SetActive(true);
        float elapsed = 0f;
        Word wordScript = GameObject.FindObjectOfType<Word>();
        //text equals the score of the word
        float scoreDisplayed = wordScript.bonusPercentage;
        //remove decimal points from below number
        if (scoreDisplayed > 0)
        {
            scoreDisplayed = scoreDisplayed * 100; 
            Debug.Log("ADDED BONUS: " + scoreDisplayed);
            tmpText.text = "+" + scoreDisplayed.ToString("F0") + "% Bonus!";
        }
        if (scoreDisplayed < 0)
        {
            scoreDisplayed = scoreDisplayed * 100;
            Debug.Log("SHOULD NEVER HAPPEN SHOULD NEVER BE LESS THAN ZERO: " + scoreDisplayed);
            tmpText.text = "" + scoreDisplayed.ToString("F0") + "% Penalty\nUse Longer Words!";
        }
        if (scoreDisplayed == 0)
        {
            Debug.Log("SHOULD BE ZERO: " + scoreDisplayed);
        }

        if (scoreDisplayed < 0)
        {
            //tmp text is red
            tmpText.color = new Color(1f, 0.2f, 0.2f, 1f); // red
        }
        else if (scoreDisplayed >= 20 && scoreDisplayed < 40)
        {
            // tmp text is yellow
            tmpText.color = new Color(1f, 1f, 0.2f, 1f); // yellow
        }
        else if (scoreDisplayed >= 40)
        {
            // tmp text is green
            tmpText.color = new Color(0.2f, 1f, 0.2f, 1f); // green 
        }

        // Store original color
        Color originalColor = tmpText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent
        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        tmpText.gameObject.SetActive(false);
    }
}
