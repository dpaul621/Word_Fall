using System.Collections;
using UnityEngine;
using TMPro;

public class WordFade : MonoBehaviour
{
    public TextMeshProUGUI niceTextMessage;
    public TextMeshProUGUI ExcellentTextMessage;
    public TextMeshProUGUI superbTextMessage;
    public TextMeshProUGUI wowTextMessage;
    public TextMeshProUGUI invalidWordTextMessage;
    public float fadeDuration = 3f; // Duration of the fade

    public void Nice()
    {
        StartCoroutine(FadeOutAndDisable(niceTextMessage));
    }
    public void Excellent()
    {
        StartCoroutine(FadeOutAndDisable(ExcellentTextMessage));   
    }
    public void Superb()
    {
        StartCoroutine(FadeOutAndDisable(superbTextMessage));   
    }
    public void Wow()
    {
        StartCoroutine(FadeOutAndDisable(wowTextMessage));  
    }
    public void InvalidWord()
    {
        StartCoroutine(FadeOutAndDisable(invalidWordTextMessage));
    }

    private IEnumerator FadeOutAndDisable(TextMeshProUGUI tmpText)
    {
        tmpText.gameObject.SetActive(true);
        float elapsed = 0f;

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