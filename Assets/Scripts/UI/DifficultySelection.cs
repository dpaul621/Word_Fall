using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public GameObject levelButtons;
    public GameObject difficultyButtons;
    public GameManager gameManager;
    public LEVELSELECTOR levelSelector;
    public GameObject title;
    public GameObject toggleObject;
    public GameObject playButton;
    public GameObject scroll;

    public float difficultySetting = 1;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //levelSelector = FindObjectOfType<LEVELSELECTOR>();
    }
    public void OnOneDifficultyModeClick()
    {
        gameManager.oneDifficultyMode = true;
        gameManager.MaxLevel = 200;
        LevelsAppear();
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
        //  Debug.Log("Difficulty set to Medium");
        LevelsAppear();
    }
    public void OnHardClick()
    {
        difficultySetting = 3;
        gameManager.Difficulty = 3;
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
        scroll.SetActive(true);
        title.SetActive(false);
        levelButtons.SetActive(true);
        toggleObject.SetActive(false);
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
        if (GameManager.Instance.oneDifficultyMode)
        {
            playButton.SetActive(false);
        }
        else
        {
            difficultyButtons.SetActive(false);
        }
    }
    public void LevelsDisappear()
    {
        foreach (Transform child in levelSelector.transform)
        {
            Destroy(child.gameObject);
        }
        scroll.SetActive(false);
        title.SetActive(true);
        HapticFeedback.Trigger();
        levelButtons.SetActive(false);
        toggleObject.SetActive(true);
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f); 
        if (GameManager.Instance.oneDifficultyMode)
        {
            playButton.SetActive(true);
        }
        else
        {
            difficultyButtons.SetActive(true);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForEndOfFrame();
        levelButtons.SetActive(false);
        difficultyButtons.SetActive(true);
    }
}
