using System.Collections;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }

    int startToRunDistance = 6;
    int checkDistance;

    public int transformSign { get; set; }

    private void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        Transform laserStartPoint = GetComponent<MonsterChip>()._laserStartPoint;
        RangedMonstersRoundAttackStateMachine instance = animator.GetBehaviour<RangedMonstersRoundAttackStateMachine>();
        instance.lineRenderer = lineRenderer;
        instance.lineRenderer.enabled = false;
        instance.laserStartPoint = laserStartPoint;
        instance.layerMask = LayerMask.GetMask("Player");
        checkDistance = startToRunDistance * startToRunDistance;
    }

    int roundAttackTransitionHash = Animator.StringToHash("roundAttack");
    int roundAttackStateHash = Animator.StringToHash("Base Layer.RoundAttacking");
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
        Vector3 playerPosition = MonstersManager.instance._player.transform.position;
        if (Vector3.SqrMagnitude(transform.position - playerPosition) <= checkDistance)
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
