using UnityEngine;

public class PlayerAttackBehaviours : StateMachineBehaviour
{
    [SerializeField]
    AttackState attackState;
    public GameObject playerWeapon { get; set; }
    public int[] stateHashes { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.SetCurrentAttackState(attackState, animator.gameObject);
        playerWeapon.SetActive(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int currentHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        for (int i = 6; i < stateHashes.Length; i++)
        {
            if (stateHashes[i] == currentHash)
                return;
        }
        playerWeapon.SetActive(false);
        GameManager.instance.SetCurrentAttackState(null, animator.gameObject);
    }
}
