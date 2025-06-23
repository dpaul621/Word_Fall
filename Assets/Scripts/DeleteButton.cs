using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeleteButton : MonoBehaviour
{
    public TMP_InputField inputFieldText;
    public void DeleteLastCharacter()
    {
        string text = inputFieldText.text;

        if (!string.IsNullOrEmpty(text))
        {
            inputFieldText.text = text.Substring(0, text.Length - 1);
        }
    }
    public void DeleteAllCharacters()
    {
        inputFieldText.text = string.Empty;
    }

}