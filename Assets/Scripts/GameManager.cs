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
    private void Awake()
    {
        levelPercentage = GMLevel / MaxLevel;
        if (Instance == null)
        {
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


}
