using System.Collections;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
    }

    public int transformSign { get; set; }

    private void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        MonsterChip chip = GetComponent<MonsterChip>();
        Transform laserStartPoint = chip._laserStartPoint;
        chip._distancePoint.center = new Vector3(0, 1, 2);
        chip._distancePoint.size = new Vector3(5f, 0.01f, 5f);
        RangedMonstersRoundAttackBehaviour instance = animator.GetBehaviour<RangedMonstersRoundAttackBehaviour>();
        instance.lineRenderer = lineRenderer;
        instance.lineRenderer.enabled = false;
        instance.laserStartPoint = laserStartPoint;
        instance.layerMask = LayerMask.GetMask("Player");
        animator.SetBool("isScreamming", false);
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
            hitTakableCount--;
            ToScream(GameManager.instance._player.transform.position);
        }
    }

}
