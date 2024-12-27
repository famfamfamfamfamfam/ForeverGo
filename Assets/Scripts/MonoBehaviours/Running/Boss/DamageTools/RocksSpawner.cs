using System.Collections;
using UnityEngine;

public class RocksSpawner : MonoBehaviour
{
    ObjectPool pool;
    string rockPrefabPath = "Stone";
    uint rockCount = 30;
    void OnEnable()
    {
        if (pool != null)
            StartCoroutine(SpawnRandomly(0.5f, 3f, 5f, 10f));
    }
    private void Start()
    {
        GameObject rockPrefab = Resources.Load<GameObject>(rockPrefabPath);
        pool = new ObjectPool(rockPrefab, rockCount, gameObject.transform);
        StartCoroutine(SpawnRandomly(0.5f, 3f, 5f, 10f));
    }

    Vector3 spawnPosition;
    IEnumerator SpawnRandomly(float minTime, float maxTime, float minHeight, float maxHeight)
    {
        while (!GameManager.instance.gameOver)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            int randomKey = Random.Range(0, GameManager.instance.maxKeyInMapRenderDictionary + 1);
            spawnPosition = GameManager.instance.mapRendered[randomKey];
            spawnPosition.y = Random.Range(minHeight, maxHeight);
            pool.ObjectFromPool(obj => obj.transform.position = spawnPosition);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
