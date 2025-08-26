using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
public class WordChecker : MonoBehaviour
{
    public float numberOfLetters = 3; 
    public float levelOfDifficulty = 1; 
    private static HashSet<string> validWords;
    public GameManager gameManagerScript;

    void Awake()
    {
        gameManagerScript = FindObjectOfType<GameManager>();
        if (gameManagerScript == null)
        {
            numberOfLetters = 3;
            Debug.LogError("GameManager not found in the scene.");
        }
        else
        {
            levelOfDifficulty = gameManagerScript.Difficulty;
        }
        StartCoroutine(LoadWordList());
    }

    private IEnumerator LoadWordList()
    {
        yield return new WaitForEndOfFrame(); 
        if(GameManager.Instance.Difficulty == 1)
        {
            numberOfLetters = 3; // Easy
        }
        else if(GameManager.Instance.Difficulty == 2)
        {
            numberOfLetters = 4; // Medium
        }
        else if(GameManager.Instance.Difficulty == 3)
        {
            numberOfLetters = 5; // Hard
        }
        else if(GameManager.Instance.Difficulty == 4)
        {
            numberOfLetters = 6; // Hard
        }
        else if(GameManager.Instance.Difficulty == 5)
        {
            numberOfLetters = 7; // Hard
        }
        else
        {
            Debug.LogWarning("Invalid level of difficulty. Defaulting to Easy.");
            numberOfLetters = 3;
        }
        validWords = new HashSet<string>();
        TextAsset wordFile = Resources.Load<TextAsset>("my_scrabble_wordlist"); 
        if (wordFile != null)
        {
            Debug.Log("Word list loaded successfully. Minimum word length: " + numberOfLetters);
            string[] words = wordFile.text.Split('\n');
            foreach (string word in words)
            {
                string clean = word.Trim().ToLower();
                if (!string.IsNullOrEmpty(clean) && clean.Length >= numberOfLetters && clean.All(char.IsLetter))
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
