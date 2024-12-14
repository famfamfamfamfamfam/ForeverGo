using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }

    public int transformSign { get; private set; }

    LineRenderer lineRenderer;
    Transform laserStartPoint;

    private void Start()
    {
        MonsterChip chip = GetComponent<MonsterChip>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = chip._rangedMonstersDefaultValues.laserWidth.value;
        lineRenderer.endWidth = chip._rangedMonstersDefaultValues.laserWidth.value;
        laserStartPoint = chip._laserStartPoint;
        chip._distancePoint.center = chip._rangedMonstersDefaultValues.colliderPositionRelativeToTheMonsterCenter;
        chip._distancePoint.size = chip._rangedMonstersDefaultValues.colliderSize;
        distance = chip._rangedMonstersDefaultValues.laserLength.value;
        interval = (int)chip._rangedMonstersDefaultValues.roarFrequency_countBySecond.value;
        layerMask = LayerMask.GetMask("Player");
        animator.SetBool("isScreamming", false);
    }

    int interval;
    int roundAttackTransitionHash = Animator.StringToHash("roundAttack");
    int roundAttackStateHash = Animator.StringToHash("Base Layer.RoundAttacking");
    IEnumerator Roar()
    {
        while (!GameManager.instance.gameOver)
        {
            yield return new WaitForSeconds(interval);
            container.TurnOnTemporaryAnimation(roundAttackTransitionHash, roundAttackStateHash);
        }
    }

    int screamTransitionHash = Animator.StringToHash("scream");
    int screamStateHash = Animator.StringToHash("Base Layer.Screaming");
    public void ToScream(Vector3 playerPosition)
    {
        if (hitTakableCount == 0)
        {
            transform.forward = playerPosition - transform.position;
            container.TurnOnTemporaryAnimation(screamTransitionHash, screamStateHash);
        }

    }

    public int hitTakableCount { get; set; }
    public void ToDiscoverPlayer()
    {
        if (hitTakableCount > 0)
        {
            MonstersManager.instance.ToDecreaseRangedMonstersHitTakableCount();
            ToScream(GameManager.instance._player.transform.position);
        }
    }

    Coroutine coroutine;
    public void OnReachNewWayPoint(int indexInWayPointsList)
    {
        animator.applyRootMotion = true;
        MonstersManager.instance.ToAttachToWayPoint(transform, indexInWayPointsList);
        transformSign = indexInWayPointsList;
        transform.rotation = MonstersManager.instance.RotationLookingToCenterPoint(transform.position);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Roar());
    }


    public void OnRoundAttackAnimationEnter()
    {
        laserStartPoint.rotation = MonstersManager.instance.RotationLookingToCenterPoint(laserStartPoint.position);
        lineRenderer.enabled = true;
    }

    RaycastHit hit;
    float distance;
    LayerMask layerMask;
    public void OnRoundAttackAnimating()
    {
        lineRenderer.SetPosition(0, laserStartPoint.position);
        lineRenderer.SetPosition(1, laserStartPoint.position + laserStartPoint.forward * distance);
        if (Physics.Raycast(laserStartPoint.position, laserStartPoint.forward, out hit, distance, layerMask))
        {
            GameManager.instance.OnAttack(animator.gameObject, hit.collider.gameObject);
        }
    }

    public void OnRoundAttackAnimationExit()
    {
        lineRenderer.enabled = false;
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }
}
