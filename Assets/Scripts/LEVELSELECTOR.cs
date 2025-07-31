using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.SocialPlatforms;

public class LEVELSELECTOR : MonoBehaviour
{

    public GameObject levelPrefab;
    public GameObject grayedOutLevelPrefab;
    //private PlayerProgressManager progressManager;
    public int levelFound;
    public int TEST;
    public GameObject loadingImage;
    void OnEnable()
    {
        StartCoroutine(CreateLevelButtons());
    }
    public void LevelOne()
    {
        SceneManager.LoadScene(1);
    }
    public void LevelSelectorButton()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.GMLevel = int.Parse
        (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text); 
    }
    public IEnumerator CreateLevelButtons()
    {
        yield return new WaitForEndOfFrame();
        // Wait for any existing loading coroutine to finish first, if necessary
        if (!LocalSaveManager.HasSaveFile())
        {
            LocalSaveManager.Save(GameManager.Instance.playerProgress);
        }
        else
        {
            LocalSaveManager.Load();
            GameManager.Instance.playerProgress = LocalSaveManager.Load();
        }

        // Always load level from updated local progress
        yield return new WaitForEndOfFrame();
        int levelFound = GameManager.Instance.GetLevelForCurrentDifficulty();
        if (levelFound <= 0) levelFound = 1;
        yield return new WaitForEndOfFrame();
        CreateLevelButtonsForReal(levelFound);
    }
    /*IEnumerator WaitForProgress()
    {
        if(progressManager.progress != null)
        {
            Debug.Log(" AT beginning : " + progressManager.progress.levelEasy + " " +
                progressManager.progress.levelMedium + " " +
                progressManager.progress.levelHard);
        }
        else
        {
            Debug.LogWarning("ProgressManager is null in WaitForProgress.");
        }

        while (progressManager.progress == null)
        {
            Debug.Log("Waiting for progress to be loaded...");
            loadingImage.SetActive(true);
            yield return null; // wait until progress is loaded
        }
        loadingImage.SetActive(false);

        if (GameManager.Instance.Difficulty == 1)
        {
            levelFound = progressManager.progress.levelEasy;
            Debug.Log("Difficulty set to Easy, level found: " + levelFound);
        }
        else if (GameManager.Instance.Difficulty == 2)
        {
            levelFound = progressManager.progress.levelMedium;
            Debug.Log("Difficulty set to Medium, level found: " + levelFound);
        }
        else if (GameManager.Instance.Difficulty == 3)
        {
            levelFound = progressManager.progress.levelHard;
            Debug.Log("Difficulty set to Hard, level found: " + levelFound);
        }
        else
        {

            Debug.LogError("Difficulty not set correctly, defaulting to Easy. Difficulty: " + GameManager.Instance.Difficulty);
            levelFound = progressManager.progress.levelEasy;
        }
        if (levelFound <= 0)
        {
            levelFound = 1;
        }


        //wait a WaitForEndOfFrame
        yield return new WaitForEndOfFrame();
        CreateLevelButtonsForReal();
    }*/
    void CreateLevelButtonsForReal(int levelUsed)
    {
        for (int i = 1; i <= levelUsed; i++)
        {
            GameObject button = Instantiate(levelPrefab);
            button.transform.SetParent(transform, false);

            TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
            textComponent.text = i.ToString();
            RectTransform uiRect = button.GetComponent<RectTransform>();

            int column = (i - 1) % 6;
            int row = (i - 1) / 6;

            float xSpacing = 200f;
            float ySpacing = 200f;

            float xPosition = column * xSpacing;
            float yPosition = -row * ySpacing; // move down each row

            uiRect.anchoredPosition = new Vector2(xPosition, yPosition);

            int level = i;
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.GMLevel = level;
                SceneManager.LoadScene(1);
                HapticFeedback.Trigger();
            });
        }
        for (int i = levelUsed + 1; i <= 40; i++)
        {
            GameObject button = Instantiate(grayedOutLevelPrefab);
            button.transform.SetParent(transform, false);

            TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
            textComponent.text = i.ToString();
            RectTransform uiRect = button.GetComponent<RectTransform>();

            int column = (i - 1) % 6;
            int row = (i - 1) / 6;

            float xSpacing = 200f;
            float ySpacing = 200f;

            float xPosition = column * xSpacing;
            float yPosition = -row * ySpacing;

            uiRect.anchoredPosition = new Vector2(xPosition, yPosition);

            // Disable the button to make it unclickable
            button.GetComponent<UnityEngine.UI.Button>().interactable = false;

            // Optionally, visually gray it out
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = button.AddComponent<CanvasGroup>();
            }
            //canvasGroup.alpha = 0.5f; // make it look disabled
        }
    }
    public void Level11()
    {
        SceneManager.LoadScene(11);
    }
    public void Level12()
    {
        SceneManager.LoadScene(12);
    }
}
