using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }

    LineRenderer lineRenderer;
    Transform laserStartPoint;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        laserStartPoint = GetComponent<MonsterChip>()._laserStartPoint;
    }

}
