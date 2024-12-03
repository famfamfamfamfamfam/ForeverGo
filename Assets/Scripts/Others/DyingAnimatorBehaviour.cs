using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingAnimatorBehaviour : StateMachineBehaviour
{
    [SerializeField]
    AnimatorOverrideController newController;
    [SerializeField]
    string[] needToActiveTransitionNames;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<MonsterChip>() != null)
        {
            animator.runtimeAnimatorController = newController;
            foreach (string name in needToActiveTransitionNames)
            {
                animator.SetBool(name, true);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Player>()?.AutoChangePlayerCharacterAsDie();
        animator.gameObject.GetComponent<MonsterChip>()?.gameObject.SetActive(false);
        if (MonstersManager.instance.monsters.Count == 0)
            GameManager.instance.gameOver = true;
    }
}
