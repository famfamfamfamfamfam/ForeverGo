using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersAttackBehaviours : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<MeleeMonsterController>()?
            .TurnDamagingTool(stateInfo.fullPathHash, animator.gameObject.GetComponent<MonsterChip>());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMonsterController controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        int currentHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        foreach (int i in controller?.attackStateHashes)
        {
            if (currentHash == i)
                return;
        }
        controller.TurnDamagingTool(stateInfo.fullPathHash, animator.gameObject.GetComponent<MonsterChip>());
    }
}
