using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateMachine : StateMachineBehaviour
{
    public GameObject playerWeapon { get; set; }
    public int[] stateHashes {  get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerWeapon.SetActive(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 6; i < stateHashes.Length; i++)
        {
            if (stateHashes[i] == animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
                return;
        }
        playerWeapon.SetActive(false);
    }
}
