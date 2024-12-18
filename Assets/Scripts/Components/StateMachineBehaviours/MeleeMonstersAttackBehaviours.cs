using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeMonstersAttackBehaviours : StateMachineBehaviour
{
    [SerializeField]
    bool isJumping;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isJumping)
            animator.SetBool("isJumping", true);
        MeleeMonsterController controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        ToTurnDamagingToolForMeleeMonsters(controller, stateInfo);
        controller.SetNewForwardVector(GameManager.instance._player.transform.position);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isJumping)
            animator.SetBool("isJumping", false);
        MeleeMonsterController controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        ToTurnDamagingToolForMeleeMonsters(controller, stateInfo);
    }

    void ToTurnDamagingToolForMeleeMonsters(MeleeMonsterController controller, AnimatorStateInfo stateInfo)
    {
        controller.TurnDamagingTool(stateInfo.fullPathHash);
    }
}
