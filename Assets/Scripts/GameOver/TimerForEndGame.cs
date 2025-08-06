using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerForEndGame : MonoBehaviour
{
    public float timerForEndGameTimer;
    public TextMeshProUGUI timerText;

    void Start()
    {
        timerForEndGameTimer = 0f;
    }

    void Update()
    {
        GameOverSpawnLocation[] gameOverSpawnLocations = FindObjectsOfType<GameOverSpawnLocation>();
        float highestTimer = 0f;

        foreach (GameOverSpawnLocation location in gameOverSpawnLocations)
        {
            if (location.timer > highestTimer)
            {
                highestTimer = location.timer;
            }
        }

        if (highestTimer > 0f)
        {
            //Debug.Log($"Highest GameOverSpawnLocation Timer: {highestTimer}");
            timerText.text = $"Letters at Top!\nTime Until Game Over:\n{10 - highestTimer:F2} seconds";
        }
        else
        {
            timerText.text = ""; // Optional: clear the text if no timers are active
        }
    }
}
