using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBehaviour : StateMachineBehaviour
{
    MonsterChip monster;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.gameObject.GetComponent<MonsterChip>();
        if (monster != null)
            animator.SetBool("isDying", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monster != null)
        {
            monster.gameObject.SetActive(false);
            if (MonstersManager.instance.monsters.Count == 0)
                GameManager.instance.SetGameOverState(GameOverState.Win);
            return;
        }

        animator.gameObject.GetComponent<Player>().AutoChangePlayerCharacterAsDie();
    }
}
