using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Difficulty = 1;
    public int GMLevel;
    public int MaxLevel = 40;
    LetterSpawnScript letterSpawnScript;
    public float levelPercentage;
    public PlayerProgress playerProgress; 
    private void Awake()
    {
        levelPercentage = GMLevel / MaxLevel;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            /*if (LocalSaveManager.HasSaveFile())
            {
                playerProgress = LocalSaveManager.Load();
            }*/
            /*else
            {
                Debug.Log("🟡 No local save found. Attempting to load from server...");
                StartCoroutine(LoadFromServerOrDefault());
            }*/
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        levelPercentage = (float)GMLevel / (float)MaxLevel;
    }
    public int GetLevelForCurrentDifficulty()
    {
        return Difficulty switch
        {
            1 => playerProgress.levelEasy,
            2 => playerProgress.levelMedium,
            3 => playerProgress.levelHard,
            _ => 1
        };
    }

    public void SetLevelForCurrentDifficulty(int level)
    {
        switch (Difficulty)
        {
            case 1: playerProgress.levelEasy = level; break;
            case 2: playerProgress.levelMedium = level; break;
            case 3: playerProgress.levelHard = level; break;
        }

        LocalSaveManager.Save(playerProgress);
        Debug.Log($"Set level for difficulty {Difficulty}: {level}");
    }
    /*IEnumerator LoadFromServerOrDefault()
    {
        var progressManager = FindObjectOfType<PlayerProgressManager>();
        Debug.Log("Loading progress from server...");
        if (progressManager == null)
        {
            Debug.LogError("PlayerProgressManager not found in scene!");
        }
        // Try to fetch from server
        yield return StartCoroutine(progressManager.LoadProgress()); 
        yield return new WaitUntil(() => progressManager.progress != null);
        
        playerProgress = progressManager.progress;

        // Save it locally for future runs
        LocalSaveManager.Save(playerProgress);

        GMLevel = GetLevelForCurrentDifficulty();
    }*/


}
