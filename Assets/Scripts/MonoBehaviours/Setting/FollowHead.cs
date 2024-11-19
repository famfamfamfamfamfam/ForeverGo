using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour, ILateUpdateMethodWaitingToRun
{
    [SerializeField]
    Transform body;
    float upwardDistance = 1.5f;
    private void Start()
    {
        SetUpPosition();
    }
    public void SetUpPosition()
    {
        transform.position = upwardDistance * body.up + body.position;
    }
}
