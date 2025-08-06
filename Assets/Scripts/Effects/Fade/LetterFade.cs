using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterFade : MonoBehaviour
{
    SpriteRenderer letterSpriteRenderer;
    public float fadeDuration = 2f; // Duration of the fade

    void OnEnable()
    {
        //Debug.Log("LetterFade script enabled for: " + gameObject.name);
        StartCoroutine(FadeOutAndDisable(letterSpriteRenderer));
    }
    private IEnumerator FadeOutAndDisable(SpriteRenderer letterSpriteRenderer)
    {
        yield return new WaitForEndOfFrame();
        letterSpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        if (parentSpriteRenderer == null)
        {
            Debug.LogWarning("Parent SpriteRenderer not found for: " + gameObject.name);
        }
        else
        {
            letterSpriteRenderer.sprite = parentSpriteRenderer.sprite;
        }
        letterSpriteRenderer.gameObject.SetActive(true);
        float elapsed = 0f;

        // Store original color
        Color originalColor = letterSpriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            letterSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent
        letterSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        letterSpriteRenderer.gameObject.SetActive(false);
    }
}
