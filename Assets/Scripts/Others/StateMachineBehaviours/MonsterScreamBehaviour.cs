using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScreamBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MonstersManager.instance.SetUpTheCube(animator.transform.position);
        MonstersManager.instance.ToTurnTheRangedMonsters();
    }
}
