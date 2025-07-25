using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class EnterWord : MonoBehaviour
{
    GameObject wordObj;
    Word wordScript;
    void Awake()
    {
        wordObj = GameObject.FindGameObjectWithTag("Word");
        if (wordObj != null)
        {
            wordScript = wordObj.GetComponent<Word>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Word' found!");
        }
    }
    public void CheckWordAndClear()
    {
        HapticFeedback.Trigger();
        string word = GetInputWord();

        if (WordChecker.IsValid(word))
        {
            ClearInputField();
            ClearWordScript();
        }
        else
        {
            Debug.Log($"❌ Invalid word: {word}");
            wordScript.RemoveAllLetters();
            ClearInputField();
            //call on word Fade script to display invalid word message
            if (wordScript.wordFadeScript != null)
            {
                wordScript.wordFadeScript.InvalidWord();
            }
            else
            {
                Debug.LogError("No WordFade script found on the GameObject with tag 'Word'.");
            }
        }
    }

    string GetInputWord()
    {
        GameObject inputObj = GameObject.FindGameObjectWithTag("TextInputField");
        if (inputObj != null)
        {
            var inputField = inputObj.GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                return inputField.text.Trim();
            }
        }
        return string.Empty;
    }

    private void ClearInputField()
    { 
        GameObject inputObj = GameObject.FindGameObjectWithTag("TextInputField");
        if (inputObj != null)
        {
            var inputField = inputObj.GetComponent<TMPro.TMP_InputField>();
            if (inputField != null)
            {
                inputField.text = string.Empty;
            }
            else
            {
                Debug.LogError("No TMP_InputField component found on the GameObject with tag 'TextInputField'.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with tag 'TextInputField' found!");
        }
    }
    private void ClearWordScript()
    { 

        if (wordObj != null)
        {
            if (wordScript != null)
            {
                wordScript.ClearLetters();
            }
            else
            {
                Debug.LogError("No Word script found on the GameObject with tag 'Word'.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Word' found!");
        }
    }

    public void EnterWordFunction()
    {
        CheckWordAndClear(); 
    }
}