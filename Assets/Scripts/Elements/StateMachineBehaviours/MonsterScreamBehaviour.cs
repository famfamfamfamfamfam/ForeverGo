using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScreamBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isScreamming", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MonstersManager.instance.SetUpTheCube(animator.transform.position);
        MonstersManager.instance.ToTurnTheRangedMonsters();
        animator.SetBool("isScreamming", false);
    }
}
