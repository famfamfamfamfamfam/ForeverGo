using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonstersFleeBehaviour : StateMachineBehaviour
{
    LayerMask mask;
    RaycastHit hit;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMonsterController meleeMonsterController = animator.gameObject.GetComponent<MeleeMonsterController>();
        mask = meleeMonsterController.combineMask;
        if (Physics.Raycast(0.25f * animator.transform.up + animator.transform.position, animator.transform.forward, out hit, 1f, mask))
        {
            Debug.Log(hit.collider.gameObject.name);
            meleeMonsterController.ToFleeOnLowHP();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.forward = MonstersManager.instance._player.transform.position - animator.transform.position;
    }
}
