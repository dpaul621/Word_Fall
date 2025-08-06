using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelNumberDisplay : MonoBehaviour
{
    public float levelNumber;
    // This script is used to display the current level number in the UI.
    //it retrieves the game level number from the scenemanager current scene
    private void Start()
    {
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            levelNumber = GameManager.Instance.GMLevel;
            textComponent.text = "Level: " + levelNumber.ToString();
        }
        else
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
        }
    }
}
