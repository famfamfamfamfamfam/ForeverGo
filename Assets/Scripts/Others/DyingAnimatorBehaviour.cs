using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingAnimatorBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isDying", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Player>()?.AutoChangePlayerCharacterAsDie();
        animator.gameObject.GetComponent<MonsterChip>()?.gameObject.SetActive(false);
        if (MonstersManager.instance.monsters.Count == 0)
            GameManager.instance.gameOver = true;
    }
}
