using TMPro;
using UnityEngine;

public class DisplayLettersPerWord : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text label;   // assign your Text (TMP) here
    WordChecker word;        // assign the Word that tracks progress
    

    [Header("Formatting")]
    //[SerializeField] private int displayNumberOfLetters = 3;
    [SerializeField] private string prefix = "Min Letters / Word: ";

    void Reset()
    {
        // Auto-fill on add
        label = GetComponent<TMP_Text>();
        if (word == null) word = FindObjectOfType<WordChecker>();
    }

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
        if (word == null)  word  = FindObjectOfType<WordChecker>();
        UpdateText();
    }

    void Update()
    {
        if (label == null || word == null) return;
        UpdateText();
    }

    private void UpdateText()
    {
        float num = word.numberOfLetters;
        // :n0 adds thousands separators (e.g., 10,000)
        label.text = $"{prefix}{num:n0}";
    }
}
