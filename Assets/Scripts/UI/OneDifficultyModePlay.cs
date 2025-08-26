using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OneDifficultyModePlay : MonoBehaviour
{
    LEVELSELECTOR levelSelector;
    public void Play()
    {
        GameManager.Instance.oneDifficultyMode = true;
        StartCoroutine(PlayButton());
    }

    public IEnumerator PlayButton()
    {
        yield return new WaitForEndOfFrame();
        levelSelector = FindObjectOfType<LEVELSELECTOR>();
        //levelSelector.LevelCreation();
    }

}
