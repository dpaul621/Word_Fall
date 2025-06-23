using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//back up for og
public class LetterSpawnScripting : MonoBehaviour
{
    public List<GameObject> letters;
    public List<float> spawnXValues;

    [Tooltip("Seconds between spawns")]
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;

    public float chanceOfAdditionalLetter = 0.2f;

    private Coroutine _spawnRoutine;
    private List<float> unusedXValues;

    void OnEnable()
    {
        unusedXValues = new List<float>(spawnXValues);
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
            RandomlySpawnLetter();
            float randomSpawnInterval = Random.Range(lowRangeSpawnInterval, highRangeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    void RandomlySpawnLetter()
    {
        // Refill if all X values have been used
        if (unusedXValues.Count == 0)
        {
            unusedXValues = new List<float>(spawnXValues);
        }

        // Pick a first unique X value and remove it from unused
        int firstIndex = Random.Range(0, unusedXValues.Count);
        float randomXValue = unusedXValues[firstIndex];
        unusedXValues.RemoveAt(firstIndex);

        GameObject randomletter = letters[Random.Range(0, letters.Count)];
        Instantiate(randomletter, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);

        // Possibly spawn a second letter, using a different X value
        if (Random.Range(0f, 1f) < chanceOfAdditionalLetter && unusedXValues.Count > 0)
        {
            int secondIndex = Random.Range(0, unusedXValues.Count);
            float secondXValue = unusedXValues[secondIndex];
            unusedXValues.RemoveAt(secondIndex);

            GameObject secondLetter = letters[Random.Range(0, letters.Count)];
            Instantiate(secondLetter, new Vector3(secondXValue, transform.position.y, 0), Quaternion.identity);
        }
    }
}