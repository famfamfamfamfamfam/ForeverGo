using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateMachine : StateMachineBehaviour
{
    public GameObject playerWeapon { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerWeapon.SetActive(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerWeapon.SetActive(false);
    }
}
