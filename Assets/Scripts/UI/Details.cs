using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Details : MonoBehaviour
{
    public GameObject detailsPanel;
    Pause pauseScript;
    public GameObject firstPage;
    public GameObject secondPage;
    public bool isFirstPage = true;

    void Awake()
    {
        pauseScript = FindObjectOfType<Pause>();
    }
    public void ViewDetails()
    {
        Time.timeScale = 0f;
        isFirstPage = true;
        detailsPanel.SetActive(true);
        pauseScript.isPaused = true;
        firstPage.SetActive(true);
        secondPage.SetActive(false);
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

    public void ResumeGame()
    {
        if (isFirstPage)
        {
            firstPage.SetActive(false);
            secondPage.SetActive(true);
            isFirstPage = false;
        }
        else
        {
            Debug.Log("Resuming game from details panel.");
            Time.timeScale = 1f;
            detailsPanel.SetActive(false);
            pauseScript.isPaused = false;
            HapticFeedback.Trigger();
            AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
        }
    }
}
