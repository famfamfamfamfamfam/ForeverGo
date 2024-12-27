using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class RocksSpawner : MonoBehaviour
{
    ObjectPool pool;
    string rockPrefabPath = "Stone.prefab";
    uint rockCount = 20;
    void Start()
    {
        GameObject rockPrefab = Resources.Load<GameObject>(rockPrefabPath);
        pool = new ObjectPool(rockPrefab, rockCount);
    }

    IEnumerator SpawnRock()
    {
        yield return null;
        
    }

}
