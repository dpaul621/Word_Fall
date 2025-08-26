using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDifficultyLengthSetting : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.oneDifficultyMode)
        {
            //if game manager instance level is between 1 and 40
            if (GameManager.Instance.GMLevel >= 1 && GameManager.Instance.GMLevel <= 40)
            {
                Debug.Log("Setting difficulty to 1 for levels 1-40.");
                // Set the length for one difficulty mode
                GameManager.Instance.Difficulty = 1;
            }
            if (GameManager.Instance.GMLevel >= 41 && GameManager.Instance.GMLevel <= 80)
            {
                Debug.Log("Setting difficulty to 2 for levels 41-80.");
                // Set the length for one difficulty mode
                GameManager.Instance.Difficulty = 2;
            }
            if (GameManager.Instance.GMLevel >= 81 && GameManager.Instance.GMLevel <= 120)
            {
                Debug.Log("Setting difficulty to 3 for levels 81-120.");
                // Set the length for one difficulty mode
                GameManager.Instance.Difficulty = 3;
            }
            if (GameManager.Instance.GMLevel >= 121 && GameManager.Instance.GMLevel <= 160)
            {
                Debug.Log("Setting difficulty to 4 for levels 121-160.");
                // Set the length for one difficulty mode
                GameManager.Instance.Difficulty = 4;
            }
            if (GameManager.Instance.GMLevel >= 161 && GameManager.Instance.GMLevel <= 200)
            {
                Debug.Log("Setting difficulty to 5 for levels 161-200.");
                // Set the length for one difficulty mode
                GameManager.Instance.Difficulty = 5;
            }
        }
    }
}
