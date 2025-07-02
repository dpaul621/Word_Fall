using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public GameObject levelButtons;
    public GameObject difficultyButtons;
    public GameObject backgroundWithTitle;
    public GameObject backgroundWithoutTitle;
    public GameManager gameManager;

    public float difficultySetting = 1;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnEasyClick()
    {
        difficultySetting = 1;
        gameManager.Difficulty = 1; 
        Debug.Log("Difficulty set to Easy");
        LevelsAppear();
    }
    public void OnMediumClick()
    {
        difficultySetting = 2;
        gameManager.Difficulty = 2;
        Debug.Log("Difficulty set to Medium");
        LevelsAppear();
    }
    public void OnHardClick()
    {
        difficultySetting = 3;
        gameManager.Difficulty = 3;
        Debug.Log("Difficulty set to Hard");
        LevelsAppear();
    }
    public void OnExperimentalClick()
    {
        difficultySetting = 1;
        gameManager.Difficulty = 1;
        Debug.Log("Difficulty set to Experimental");
        SceneManager.LoadScene(12);
    }
    void LevelsAppear()
    {
        levelButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        backgroundWithoutTitle.SetActive(true);
        backgroundWithTitle.SetActive(false);
    }

    public void LevelsDisappear()
    {
        levelButtons.SetActive(false);
        difficultyButtons.SetActive(true);
        backgroundWithoutTitle.SetActive(false);
        backgroundWithTitle.SetActive(true);
    }
}
