using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{
    public List<GameObject> cloudPrefabs;
    public float spawnInterval = 2f;
    public float spawnX = 10f;
    private readonly List<float> spawnY = new() { 9f, 7f, 5f, 3f, 1f, -1f };

    private readonly Queue<int> recent = new();

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            int yIndex = PickYIndex();
            float y = spawnY[yIndex];

            Vector2 position = new Vector2(spawnX, y);

            GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
            GameObject cloud = Instantiate(prefab, position, Quaternion.identity);
            cloud.transform.SetParent(transform); // or null if you prefer world space

            //Debug.Log($"Cloud Spawned at y={y}");
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    int PickYIndex()
    {
        // Build candidate list excluding the last 3 used
        var candidates = new List<int>();
        for (int i = 0; i < spawnY.Count; i++)
        {
            if (!recent.Contains(i))
                candidates.Add(i);
        }

        // Safety: if somehow there are no candidates, clear the oldest
        if (candidates.Count == 0)
        {
            if (recent.Count > 0) recent.Dequeue();
            for (int i = 0; i < spawnY.Count; i++)
                if (!recent.Contains(i)) candidates.Add(i);
        }

        int choice = candidates[Random.Range(0, candidates.Count)];

        // Record this choice
        recent.Enqueue(choice);
        if (recent.Count > 3) recent.Dequeue();

        return choice;
    }
}
