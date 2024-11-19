using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateUpdateRunOrder : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objsWaitingToRun;
    List<ILateUpdateMethodWaitingToRun> methodsWaitingToRun;
    private void Start()
    {
        methodsWaitingToRun = new List<ILateUpdateMethodWaitingToRun>();
        foreach (GameObject obj in objsWaitingToRun)
        {
            methodsWaitingToRun.Add(obj.GetComponent<ILateUpdateMethodWaitingToRun>());
        }
    }
    private void LateUpdate()
    {
        foreach (ILateUpdateMethodWaitingToRun method in methodsWaitingToRun)
        {
            method.SetUpPosition();
        }
    }
}
