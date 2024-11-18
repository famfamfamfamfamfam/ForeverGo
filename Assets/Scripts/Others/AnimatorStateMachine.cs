using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateMachine : StateMachineBehaviour
{
    public GameObject playerWeapon { get; set; }
    public PlayInput input { get; set; }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        input.DeactiveWeaponOnAnimationExit(playerWeapon);
    }
}
