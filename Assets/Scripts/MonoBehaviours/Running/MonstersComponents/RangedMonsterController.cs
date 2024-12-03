using System.Collections;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }

    int startToRunDistance = 7;
    int checkDistance;

    public int transformSign { get; set; }

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
        checkDistance = startToRunDistance * startToRunDistance;
    }

    int roundAttackTransitionHash = Animator.StringToHash("roundAttack");
    int roundAttackStateHash = Animator.StringToHash("RoundAttacking");
    public IEnumerator Roar()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            container.TurnOnTemporaryAnimation(roundAttackTransitionHash, roundAttackStateHash);
        }
    }

    private void Update()
    {
        if (MonstersManager.instance.CheckDistanceToPlayer(transform, checkDistance))
        {
            MonstersManager.instance.ToTurnTheRangedMonsters();
        }
    }

    int runTransitionHash = Animator.StringToHash("isRunning");
    public void ToRun()
    {
        container.StartLoopAnimation(runTransitionHash);
    }
    public void ToStopRunning()
    {
        container.StopLoopAnimation(runTransitionHash);
    }
}
