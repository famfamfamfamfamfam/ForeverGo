using UnityEngine;

public class RangedMonstersRoundAttackBehaviour : StateMachineBehaviour
{
    public LineRenderer lineRenderer { get; set; }
    public Transform laserStartPoint { get; set; }
    RaycastHit hit;
    float distance = 12;
    public LayerMask layerMask { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        laserStartPoint.rotation = MonstersManager.instance.RotationLookingToCenterPoint(laserStartPoint.position);
        lineRenderer.enabled = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lineRenderer.SetPosition(0, laserStartPoint.position);
        lineRenderer.SetPosition(1, laserStartPoint.position + laserStartPoint.forward * distance);
        if (Physics.Raycast(laserStartPoint.position, laserStartPoint.forward, out hit, distance, layerMask))
        {
            GameManager.instance.OnAttack(animator.gameObject, hit.collider.gameObject);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lineRenderer.enabled = false;
    }
}
