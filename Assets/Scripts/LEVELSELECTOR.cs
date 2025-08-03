using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LEVELSELECTOR : MonoBehaviour
{

    public GameObject levelPrefab;
    public GameObject grayedOutLevelPrefab;
    public int levelFound;
    public int TEST;
    public GameObject loadingImage;
    public Toggle isBetaToggle;


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
        if (isBetaToggle.isOn)
        {
            yield return new WaitForEndOfFrame();
            int levelFound = 100;
            if (levelFound <= 0) levelFound = 1;
            yield return new WaitForEndOfFrame();
            CreateLevelButtonsForReal(levelFound);
        }
        else
        {
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

    }
    void CreateLevelButtonsForReal(int levelUsed)
    {
        for (int i = 1; i <= levelUsed; i++)
        {
            GameObject button = Instantiate(levelPrefab);
            button.transform.SetParent(transform, false);

            TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
            textComponent.text = i.ToString();
            RectTransform uiRect = button.GetComponent<RectTransform>();

            int column = (i - 1) % 7;
            int row = (i - 1) / 7;

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
        for (int i = levelUsed + 1; i <= 100; i++)
        {
            GameObject button = Instantiate(grayedOutLevelPrefab);
            button.transform.SetParent(transform, false);

            TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
            textComponent.text = i.ToString();
            RectTransform uiRect = button.GetComponent<RectTransform>();

            int column = (i - 1) % 7;
            int row = (i - 1) / 7;

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
