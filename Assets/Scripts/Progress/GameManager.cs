using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Difficulty = 1;
    public int GMLevel;
    public int MaxLevel = 100;
    public float levelPercentage;
    public PlayerProgress playerProgress; 
    private void Awake()
    {
        levelPercentage = GMLevel / MaxLevel;
        if (Instance == null)
        {
            Debug.Log($"DDOL on {name}. IsRoot={(transform.parent == null)}", this);
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
}
