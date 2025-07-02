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
    private float? lastXValue = null;
    public float chancesOfABomb = 0.1f;
    public float chancesOfElectic = 0.0f;
    Letter letterScript;
    public float level;

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

    void LevelSetter()
    {
        earlyGameTimer = 12f + (level * 2f); 
        lowRangeSpawnInterval = 3f - (level * 0.1f);
        highRangeSpawnInterval = 4f - (level * 0.1f);
        chanceOfAdditionalLetter = 0.01f + (level * 0.005f);
        letterMovementSpeed = -0.175f - (level * 0.01f);
        earlyLowRangeSpawnInterval = 1f;
        earlyHighRangeSpawnInterval = 2f;
        earlyChanceOfAdditionalLetter = 0.11f;
        earlyLetterMovementSpeed = -0.35f;
    }

    void Start()
    {
        level = GameManager.Instance.GMLevel;
        LevelSetter();
        if(level >= 1 && level <= 10)
        {
            chancesOfABomb = 0f;
        }
        //if level is between 1 and 20, chances of electric is 0.0f
        if(level >= 1 && level <= 20)
        {
            chancesOfElectic = 0f;
        }
    }
    void FixedUpdate()
    {
        //for the first 12 seconds of the game, low range spawn interval, high range spawn interval, chance of additional letter and movement speed will be different values
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
        // Create a list of available X values, excluding the last used one
        List<float> availableX = new List<float>(spawnXValues);
        if (lastXValue.HasValue)
            availableX.Remove(lastXValue.Value);

        // Pick the first letter's X position
        float randomXValue = availableX[Random.Range(0, availableX.Count)];
        lastXValue = randomXValue; // Update last used X

        LetterData selectedLetter = GetWeightedRandomLetter();
        Instantiate(selectedLetter.prefab, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);
        //access the selected letter's letter script, make the selected letters 'letterisabomb' script = true 50% of the time
        selectedLetter.prefab.GetComponent<Letter>().letterIsBomb = Random.Range(0f, 1f) < chancesOfABomb;
        if (selectedLetter.prefab.GetComponent<Letter>().letterIsBomb == false)
        {
            selectedLetter.prefab.GetComponent<Letter>().isElectric = Random.Range(0f, 1f) < chancesOfElectic;
        }
        // For the possible second letter, exclude the first X value
            if (Random.Range(0f, 1f) < currentChanceOfAdditionalLetter)
            {
                List<float> secondAvailableX = new List<float>(spawnXValues);
                secondAvailableX.Remove(randomXValue);
                float secondX = secondAvailableX[Random.Range(0, secondAvailableX.Count)];
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
