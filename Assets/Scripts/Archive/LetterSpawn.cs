using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSpawn : MonoBehaviour
{
    //list of letters to spawn
    public List<GameObject> letters;
    public List<float> spawnXValues;

    [Tooltip("Seconds between spawns")]
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;

    float randomSpawnInterval;
    GameObject randomletter;
    public float chanceOfAdditionalLetter = 0.2f;

    private Coroutine _spawnRoutine;

    void OnEnable()
    {
        // Start spawning as soon as this object is enabled
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        // Clean up when disabled
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            RandomlySpawnLetter();
            randomSpawnInterval = Random.Range(lowRangeSpawnInterval, highRangeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    void RandomlySpawnLetter()
    {
        randomletter = letters[Random.Range(0, letters.Count)];
        float randomXValue = spawnXValues[Random.Range(0, spawnXValues.Count)];
        if (Random.Range(0f, 1f) < chanceOfAdditionalLetter)
        {
            List<float> availableXValues = new List<float>(spawnXValues);
            availableXValues.Remove(randomXValue);
            GameObject secondLetter = letters[Random.Range(0, letters.Count)];
            float secondXValue = availableXValues[Random.Range(0, availableXValues.Count)];
            Instantiate(secondLetter, new Vector3(secondXValue, transform.position.y, 0), Quaternion.identity);
        }
        Instantiate(randomletter, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);
    }
}
