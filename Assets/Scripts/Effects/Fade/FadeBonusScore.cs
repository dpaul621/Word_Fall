using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBonusScore : MonoBehaviour
{

    SpriteRenderer bonusScoreSpriteRenderer;
    public float fadeDuration = 2f; // Duration of the fade

    void OnEnable()
    {
        StartCoroutine(FadeOutAndDisable(bonusScoreSpriteRenderer));
    }
    private IEnumerator FadeOutAndDisable(SpriteRenderer bonusScoreSpriteRenderer)
    {
        yield return new WaitForEndOfFrame();
        bonusScoreSpriteRenderer = GetComponent<SpriteRenderer>();
        bonusScoreSpriteRenderer.gameObject.SetActive(true);
        float elapsed = 0f;

        // Store original color
        Color originalColor = bonusScoreSpriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            bonusScoreSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent
        bonusScoreSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        bonusScoreSpriteRenderer.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
