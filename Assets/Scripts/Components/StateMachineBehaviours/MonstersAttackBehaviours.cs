using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonstersAttackBehaviours : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ToTurnDamagingToolForMeleeMonsters(animator, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ToTurnDamagingToolForMeleeMonsters(animator, stateInfo);
    }

    void ToTurnDamagingToolForMeleeMonsters(Animator animator, AnimatorStateInfo stateInfo)
    {
        animator.gameObject.GetComponent<MeleeMonsterController>()
            .TurnDamagingTool(stateInfo.fullPathHash);
    }
}
