using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class LEVELSELECTOR : MonoBehaviour
{

    public GameObject levelPrefab;
    void Start()
    {
        CreateLevelButtons();
    }
    public void LevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelSelectorButton()
    {
        int level = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text);
        SceneManager.LoadScene(level);
    }
    public void CreateLevelButtons()
    {
        for (int i = 1; i <= 12; i++)
        {
            GameObject button = Instantiate(levelPrefab);
            button.transform.SetParent(transform, false);
            TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
            textComponent.text = (i).ToString();
            RectTransform uiRect = button.GetComponent<RectTransform>();

            float xAdjuster = 0;
            float iDividedByTen = (i / 10);
            iDividedByTen = Mathf.Floor(iDividedByTen);
            xAdjuster = iDividedByTen * 200;
            float yAdjuster = 0;
            int lastNumberOfi = i % 10;
            yAdjuster = lastNumberOfi * 200;

            uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x + xAdjuster, uiRect.anchoredPosition.y - (yAdjuster)); 
            // add a listener to the button to load the level when clicked
            int level = i;
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                SceneManager.LoadScene(level);
                Debug.Log("Level " + (level) + " selected");
            });
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
