using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text label;   // assign your Text (TMP) here
    Word word;        // assign the Word that tracks progress

    [Header("Formatting")]
    [SerializeField] private int displayMultiplier = 100; // 1 => "5 / 10", 100 => "500 / 1000"
    [SerializeField] private string prefix = "Score: ";

    void Reset()
    {
        // Auto-fill on add
        label = GetComponent<TMP_Text>();
        if (word == null) word = FindObjectOfType<Word>();
    }

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
        if (word == null)  word  = FindObjectOfType<Word>();
        UpdateText();
    }

    void Update()
    {
        if (label == null || word == null) return;
        UpdateText();
    }

    private void UpdateText()
    {
        int top    = Mathf.RoundToInt(word.lettersCleared * displayMultiplier);
        int bottom = Mathf.RoundToInt(word.amountOfLettersToAdvance * displayMultiplier);

        // Don’t let the numerator exceed the goal visually
        top = Mathf.Clamp(top, 0, bottom);

        // :n0 adds thousands separators (e.g., 10,000)
        label.text = $"{prefix}{top:n0} / {bottom:n0}";
    }
}
