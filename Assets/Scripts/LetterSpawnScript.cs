using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LetterSpawnScript : MonoBehaviour
{
    public List<LetterData> letterDataList;
    public List<float> spawnXValues;
    public float earlyGameTimer = 12f;
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;
    public float chanceOfAdditionalLetter = 0.2f;
    public float letterMovementSpeed;
    public float earlyLowRangeSpawnInterval = 0.5f;
    public float earlyHighRangeSpawnInterval = 1.5f;
    public float earlyChanceOfAdditionalLetter = 0.1f;
    public float earlyLetterMovementSpeed = 2f;
    float currentLowRangeSpawnInterval;
    float currentHighRangeSpawnInterval;
    float currentChanceOfAdditionalLetter;
    public float currentLetterMovementSpeed;
    private Coroutine _spawnRoutine;

    void OnEnable()
    {
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }
    void Awake()
    {
            currentLowRangeSpawnInterval = earlyLowRangeSpawnInterval;
            currentHighRangeSpawnInterval = earlyHighRangeSpawnInterval;
            currentChanceOfAdditionalLetter = earlyChanceOfAdditionalLetter;
            currentLetterMovementSpeed = earlyLetterMovementSpeed;
    }
    void FixedUpdate()
    {
        //for the first 12 seconds of the game, low ran hight change chance of additional and movement speed wil be different values
        if (Time.time < earlyGameTimer)
        {
            currentLowRangeSpawnInterval = earlyLowRangeSpawnInterval;
            currentHighRangeSpawnInterval = earlyHighRangeSpawnInterval;
            currentChanceOfAdditionalLetter = earlyChanceOfAdditionalLetter;
            currentLetterMovementSpeed = earlyLetterMovementSpeed;
        }   
        if (Time.time > earlyGameTimer)
        {
            if (currentChanceOfAdditionalLetter != chanceOfAdditionalLetter || 
                currentLowRangeSpawnInterval != lowRangeSpawnInterval || 
                currentHighRangeSpawnInterval != highRangeSpawnInterval || 
                currentLetterMovementSpeed != letterMovementSpeed)
            {
                Debug.Log("Updating spawn parameters after early game timer.");
                currentLowRangeSpawnInterval = lowRangeSpawnInterval;
                currentHighRangeSpawnInterval = highRangeSpawnInterval;
                currentChanceOfAdditionalLetter = chanceOfAdditionalLetter;
                currentLetterMovementSpeed = letterMovementSpeed;    
            }
        }
    }
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnLetterFromWeightedList();
            float randomSpawnInterval = Random.Range(currentLowRangeSpawnInterval, currentHighRangeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    void SpawnLetterFromWeightedList()
    {
        LetterData selectedLetter = GetWeightedRandomLetter();
        float randomXValue = spawnXValues[Random.Range(0, spawnXValues.Count)];
        Instantiate(selectedLetter.prefab, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);

        if (Random.Range(0f, 1f) < currentChanceOfAdditionalLetter)
        {
            List<float> availableX = new List<float>(spawnXValues);
            availableX.Remove(randomXValue);
            float secondX = availableX[Random.Range(0, availableX.Count)];
            LetterData secondLetter = GetWeightedRandomLetter();
            Instantiate(secondLetter.prefab, new Vector3(secondX, transform.position.y, 0), Quaternion.identity);
        }
    }

    LetterData GetWeightedRandomLetter()
    {
        float totalWeight = 0f;
        foreach (var letter in letterDataList)
            totalWeight += letter.frequency;

        float rand = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var letter in letterDataList)
        {
            cumulative += letter.frequency;
            if (rand <= cumulative)
                return letter;
        }

        return letterDataList[letterDataList.Count - 1]; // fallback
    }
}
