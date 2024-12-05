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
        RangedMonstersRoundAttackBehaviour instance = animator.GetBehaviour<RangedMonstersRoundAttackBehaviour>();
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

    float adjustDistance;
    public bool stopChecking { get; set; }
    private void Update()
    {
        Vector3 playerPosition = MonstersManager.instance._player.transform.position;
        if (Time.frameCount % 200 == 0)
            adjustDistance = Random.Range(-20f, 24f);
        if (!stopChecking &&
            Vector3.SqrMagnitude(transform.position - playerPosition) <= checkDistance + adjustDistance
            && MonstersManager.instance.rangedMonstersHitTakableCount == 0)
        {
            ToScream(playerPosition);
        }
        
    }

    int screamTransitionHash = Animator.StringToHash("scream");
    int screamStateHash = Animator.StringToHash("Base Layer.Screaming");
    public void ToScream(Vector3 playerPosition)
    {
        transform.forward = playerPosition - transform.position;
        container.TurnOnTemporaryAnimation(screamTransitionHash, screamStateHash);
        stopChecking = true;
    }



    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
