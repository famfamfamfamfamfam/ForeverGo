using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    List<GameObject> objs;
    GameObject obj;
    public ObjectPool(GameObject spawnedObject, uint amount, Transform parent = null)
    {
        objs = new List<GameObject>();
        obj = spawnedObject;
        CreatePool(amount, parent);
    }

    void CreatePool(uint amount, Transform parent)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject gObj = UnityEngine.Object.Instantiate(obj, parent);
            gObj.SetActive(false);
            objs.Add(gObj);
        }
    }

    public GameObject ObjectFromPool(Action<GameObject> SetUp)
    {
        foreach (GameObject obj in objs)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                SetUp?.Invoke(obj);
                return obj;
            }
        }
        GameObject gObj = UnityEngine.Object.Instantiate(obj);
        objs.Add(gObj);
        SetUp?.Invoke(gObj);
        return gObj;
    }
}
