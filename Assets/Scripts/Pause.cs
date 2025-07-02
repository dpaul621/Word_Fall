using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;
    public void PauseGame()
    {
        Time.timeScale = 0f; 
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; 
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }

}
