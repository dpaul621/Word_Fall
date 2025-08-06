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
    public LEVELSELECTOR levelSelector;
    public GameObject toggleObject;

    public float difficultySetting = 1;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //levelSelector = FindObjectOfType<LEVELSELECTOR>();
    }
    public void OnEasyClick()
    {
        difficultySetting = 1;
        gameManager.Difficulty = 1;
        Debug.Log("Difficulty set to Easy");
        LevelsAppear();
        HapticFeedback.Trigger();
    }
    public void OnMediumClick()
    {
        difficultySetting = 2;
        gameManager.Difficulty = 2;
        HapticFeedback.Trigger();
        //  Debug.Log("Difficulty set to Medium");
        LevelsAppear();
    }
    public void OnHardClick()
    {
        difficultySetting = 3;
        gameManager.Difficulty = 3;
        HapticFeedback.Trigger();
        LevelsAppear();
    }
    public void OnExperimentalClick()
    {
        difficultySetting = 1;
        gameManager.Difficulty = 1;
        //    Debug.Log("Difficulty set to Experimental");
        SceneManager.LoadScene(12);
    }
    void LevelsAppear()
    {
        levelButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        backgroundWithoutTitle.SetActive(true);
        backgroundWithTitle.SetActive(false);
        toggleObject.SetActive(false);
    }

    void Update()
    {
        //levelSelector = FindObjectOfType<LEVELSELECTOR>();
    }
    public void LevelsDisappear()
    {
        foreach (Transform child in levelSelector.transform)
        {
            Destroy(child.gameObject);
        }
        HapticFeedback.Trigger();
        levelButtons.SetActive(false);
        difficultyButtons.SetActive(true);
        backgroundWithoutTitle.SetActive(false);
        backgroundWithTitle.SetActive(true);
        toggleObject.SetActive(true);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForEndOfFrame();
        levelButtons.SetActive(false);
        difficultyButtons.SetActive(true);
        backgroundWithoutTitle.SetActive(false);
        backgroundWithTitle.SetActive(true);
    }
}
