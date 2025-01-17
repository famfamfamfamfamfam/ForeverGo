using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonstersFleeBehaviour : StateMachineBehaviour
{
    MeleeMonsterController meleeMonsterController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeMonsterController = animator.gameObject.GetComponent<MeleeMonsterController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeMonsterController.OnFleeAnimating();
    }
}
