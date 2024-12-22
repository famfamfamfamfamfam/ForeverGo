using UnityEngine;

public class RangedMonstersRoundAttackBehaviour : StateMachineBehaviour
{
    RangedMonsterController rangedMonsterController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rangedMonsterController == null)
            rangedMonsterController = animator.gameObject.GetComponent<RangedMonsterController>();
        rangedMonsterController.OnRoundAttackAnimationEnter();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rangedMonsterController.OnRoundAttackAnimating();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rangedMonsterController.OnRoundAttackAnimationExit();
    }
}
