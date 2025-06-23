using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordChecker : MonoBehaviour
{
    private static HashSet<string> validWords;

    void Awake()
    {
        LoadWordList();
    }

    void LoadWordList()
    {
        validWords = new HashSet<string>();
        TextAsset wordFile = Resources.Load<TextAsset>("my_scrabble_wordlist"); // no .txt
        if (wordFile != null)
        {
            Debug.Log("Word list loaded successfully.");
            string[] words = wordFile.text.Split('\n');
            foreach (string word in words)
            {
                string clean = word.Trim().ToLower();
                if (!string.IsNullOrEmpty(clean) && clean.Length >= 4 && clean.All(char.IsLetter))
                    validWords.Add(clean);
            }
        }
        else
        {
            Debug.LogError("Word list file not found in Resources.");
        }
    }

    public static bool IsValid(string input)
    {
        if (validWords == null)
        {
            Debug.LogWarning("WordChecker not initialized.");
            return false;
        }
        Debug.Log($"Checking word: {input}");
        return validWords.Contains(input.ToLower());
    }
}
