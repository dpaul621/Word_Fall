using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LetterSpawnScript : MonoBehaviour
{
    public List<LetterData> letterDataList;
    public List<float> spawnXValues;
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;
    public float chanceOfAdditionalLetter = 0.2f;
    public float letterMovementSpeed;

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

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnLetterFromWeightedList();
            float randomSpawnInterval = Random.Range(lowRangeSpawnInterval, highRangeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    void SpawnLetterFromWeightedList()
    {
        LetterData selectedLetter = GetWeightedRandomLetter();
        float randomXValue = spawnXValues[Random.Range(0, spawnXValues.Count)];
        Instantiate(selectedLetter.prefab, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);

        if (Random.Range(0f, 1f) < chanceOfAdditionalLetter)
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
