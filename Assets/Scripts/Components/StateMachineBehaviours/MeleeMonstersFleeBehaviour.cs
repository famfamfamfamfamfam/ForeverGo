using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonstersFleeBehaviour : StateMachineBehaviour
{
    LayerMask mask;

    MeleeMonsterController meleeMonsterController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeMonsterController = animator.gameObject.GetComponent<MeleeMonsterController>();
        mask = meleeMonsterController.combineMask;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics.Raycast(0.25f * animator.transform.up + animator.transform.position, animator.transform.forward, 1f, mask))
        {
            meleeMonsterController.ToFleeOnLowHP();
        }
    }
}
