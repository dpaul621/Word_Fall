using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeImageAndDestroy : MonoBehaviour
{
    public Image levelImage;                  // UI Image to fade
    public TextMeshProUGUI levelText;         // TMP Text to fade
    public float fadeDuration = 1f;           // Duration of the fade

    public void FadeImage()
    {
        StartCoroutine(FadeOutAndDestroy());
    }
    void Start()
    {
        StartCoroutine(FadeOutAndDestroy());
    }
    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;

        // Store original colors
        Color imageColor = levelImage != null ? levelImage.color : Color.clear;
        Color textColor = levelText != null ? levelText.color : Color.clear;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            if (levelImage != null)
                levelImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);

            if (levelText != null)
                levelText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

            yield return null;
        }

        // Final cleanup
        if (levelImage != null)
            levelImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0f);

        if (levelText != null)
            levelText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);

        Destroy(gameObject); // Destroy this GameObject (assumes image and text are on the same GameObject)
    }
}
