using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehaviour : StateMachineBehaviour
{
    public CapsuleCollider playerCapsuleCollider { get; set; }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCapsuleCollider.enabled = false;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCapsuleCollider.enabled = true;
    }
}
