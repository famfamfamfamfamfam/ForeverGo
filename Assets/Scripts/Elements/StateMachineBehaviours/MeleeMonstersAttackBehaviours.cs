using UnityEngine;

public class MeleeMonstersAttackBehaviours : StateMachineBehaviour
{
    [SerializeField]
    bool isJumping;

    MeleeMonsterController controller;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isJumping)
            animator.SetBool("isJumping", true);
        if (controller == null)
            controller = animator.gameObject.GetComponent<MeleeMonsterController>();
        ToTurnDamagingToolForMeleeMonsters(stateInfo);
        controller.SetNewForwardVector(GameManager.instance._player.transform.position);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isJumping)
            animator.SetBool("isJumping", false);
        ToTurnDamagingToolForMeleeMonsters(stateInfo);
    }

    void ToTurnDamagingToolForMeleeMonsters(AnimatorStateInfo stateInfo)
    {
        controller.TurnDamagingTool(stateInfo.fullPathHash);
    }
}
