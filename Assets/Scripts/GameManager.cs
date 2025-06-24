using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject levelButtons;
    public GameObject difficultyButtons;

    public float Difficulty = 1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnEasyClick()
    {
        Difficulty = 1;
        Debug.Log("Difficulty set to Easy");
        LevelsAppear();
    }
    public void OnMediumClick()
    {
        Difficulty = 2;
        Debug.Log("Difficulty set to Medium");
        LevelsAppear();
    }
    public void OnHardClick()
    {
        Difficulty = 3;
        Debug.Log("Difficulty set to Hard");
        LevelsAppear();
    }
    public void OnExperimentalClick()
    {
        Difficulty = 2;
        Debug.Log("Difficulty set to Experimental");
        SceneManager.LoadScene(11);
    }
    void LevelsAppear()
    {
        if (levelButtons != null)
        {
            levelButtons.SetActive(true);
            Debug.Log("Level buttons are now visible.");
        }
        else
        {
            Debug.LogError("Level buttons GameObject is not assigned.");
        }
        if (difficultyButtons != null)
        {
            difficultyButtons.SetActive(false);
            Debug.Log("Difficulty buttons are now hidden.");
        }
        else
        {
            Debug.LogError("Difficulty buttons GameObject is not assigned.");
        }
    }   
}
