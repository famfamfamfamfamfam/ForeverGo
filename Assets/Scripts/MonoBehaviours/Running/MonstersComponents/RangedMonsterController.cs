using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }
 
    private void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.gray;
        Transform laserStartPoint = GetComponent<MonsterChip>()._laserStartPoint;
        RangedMonstersRoundAttackStateMachine instance = animator.GetBehaviour<RangedMonstersRoundAttackStateMachine>();
        instance.lineRenderer = lineRenderer;
        instance.lineRenderer.enabled = false;
        instance.laserStartPoint = laserStartPoint;
        instance.layerMask = LayerMask.GetMask("Player");
    }

    IEnumerator Roar()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            //animate
        }
    }

}
