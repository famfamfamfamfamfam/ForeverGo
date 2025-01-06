using System.Collections.Generic;
using UnityEngine;

public class RocksSpawner : MonoBehaviour
{
    ObjectPool pool;
    string rockPrefabPath = "Stone";
    uint rockCount = 40;
    private void Start()
    {
        GameObject rockPrefab = Resources.Load<GameObject>(rockPrefabPath);
        pool = new ObjectPool(rockPrefab, rockCount, gameObject.transform);
        randomKeys = new HashSet<int>();
    }

    Vector3 spawnPosition;
    HashSet<int> randomKeys;
    public void SpawnRandomly(float minHeight, float maxHeight, int currentRockCountToSpawn)
    {
        while (randomKeys.Count < currentRockCountToSpawn)
        {
            randomKeys.Add(Random.Range(0, GameManager.instance.maxKeyInMapRenderDictionary + 1));
        }
        foreach (int key in randomKeys)
        {
            spawnPosition = GameManager.instance.mapRendered[key];
            spawnPosition.y = Random.Range(minHeight, maxHeight);
            pool.ObjectFromPool(obj => obj.transform.position = spawnPosition);
        }
        randomKeys.Clear();
    }
}
