using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonstersAttackBehaviours : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMonsterController controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        ToTurnDamagingToolForMeleeMonsters(controller, stateInfo);
        controller.SetNewForwardVector(MonstersManager.instance._player.transform.position);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMonsterController controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        ToTurnDamagingToolForMeleeMonsters(controller, stateInfo);
    }

    void ToTurnDamagingToolForMeleeMonsters(MeleeMonsterController controller, AnimatorStateInfo stateInfo)
    {
        controller.TurnDamagingTool(stateInfo.fullPathHash);
    }
}
