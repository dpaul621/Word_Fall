using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;
    public GameObject volumeAdjustmentUI;
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

    public void OpenVolumeAdjustment()
    {
        // Open the volume adjustment UI
        volumeAdjustmentUI.SetActive(true);
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

    public void CloseVolumeAdjustment()
    {
        volumeAdjustmentUI.SetActive(false);
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
    }

}
