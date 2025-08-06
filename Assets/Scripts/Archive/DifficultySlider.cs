using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : MonoBehaviour
{
    public Slider scaleSlider;
    public LetterSpawnScript letterSpawnScript;

    void Start()
    {
        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(scaleSlider.value); // Set initial scale
    }

    void OnSliderValueChanged(float value)
    {
        ChangeDifficulty(value);
    }

    void ChangeDifficulty(float value)
    {
        letterSpawnScript.currentLowRangeSpawnInterval = Mathf.Lerp(letterSpawnScript.lowRangeSpawnInterval, letterSpawnScript.lowRangeSpawnInterval - 2f, value);
        letterSpawnScript.currentHighRangeSpawnInterval = Mathf.Lerp(letterSpawnScript.highRangeSpawnInterval, letterSpawnScript.highRangeSpawnInterval - 2f, value);
        letterSpawnScript.currentChanceOfAdditionalLetter = Mathf.Lerp(letterSpawnScript.chanceOfAdditionalLetter, letterSpawnScript.chanceOfAdditionalLetter + 0.2f, value);
        letterSpawnScript.currentLetterMovementSpeed = Mathf.Lerp(letterSpawnScript.letterMovementSpeed, letterSpawnScript.letterMovementSpeed - 0.15f, value);
    }
}

