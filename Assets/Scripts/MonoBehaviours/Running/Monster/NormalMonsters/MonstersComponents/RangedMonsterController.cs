using System.Collections;
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
        if (hitTakableCount == 0 && !animator.GetBool("isScreamming"))
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
            GameManager.instance.Notify(TypeOfEvent.RangedMonstersHittableCountChange, hitTakableCount);
            ToScream(GameManager.instance._player.transform.position);
        }
    }

    Coroutine coroutine;
    public void OnReachNewWayPoint(int indexInWayPointsList)
    {
        MonstersManager.instance.ToAttachToWayPoint(transform, indexInWayPointsList);
        transformSign = indexInWayPointsList;
        transform.rotation = MonstersManager.instance.RotationLookingToCenterPoint(transform.position);
        animator.applyRootMotion = true;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Roar());
    }


    public void OnRoundAttackAnimationEnter()
    {
        laserStartPoint.localRotation = Quaternion.identity;
        Debug.Log(laserStartPoint.localRotation);
        lineRenderer.enabled = true;
    }

    RaycastHit[] hit = new RaycastHit[1];
    float distance;
    LayerMask layerMask;
    public void OnRoundAttackAnimating()
    {
        lineRenderer.SetPosition(0, laserStartPoint.position);
        lineRenderer.SetPosition(1, laserStartPoint.position + laserStartPoint.forward * distance);
        if (Physics.RaycastNonAlloc(laserStartPoint.position, laserStartPoint.forward, hit, distance, layerMask) > 0)
        {
            GameManager.instance.OnAttack(animator.gameObject, hit[0].collider.gameObject);
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
