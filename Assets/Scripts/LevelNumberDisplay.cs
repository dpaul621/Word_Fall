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
    private void Awake()
    {
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            levelNumber = currentScene.buildIndex; // Assuming the build index corresponds to the level number
            textComponent.text = "Level: " + levelNumber.ToString();
        }
        else
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
        }
    }
 
    //access the
    /*private void Start()
    {
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                levelNumber = gameManager.Difficulty;
                textComponent.text = "Level: " + levelNumber.ToString();
            }
            else
            {
                Debug.LogError("GameManager not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
        }
    }*/
}
