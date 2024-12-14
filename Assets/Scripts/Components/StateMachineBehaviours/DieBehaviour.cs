using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<MonsterChip>() != null)
            animator.SetBool("isDying", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Player>()?.AutoChangePlayerCharacterAsDie();
        animator.gameObject.GetComponent<MonsterChip>()?.gameObject.SetActive(false);//chi phi ko can thiet
        if (MonstersManager.instance.monsters.Count == 0)
            GameManager.instance.SetGameOverState(true);
    }
}
