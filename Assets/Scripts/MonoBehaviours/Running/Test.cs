using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PlayInput inputMoving;
    Animator animator;
    [SerializeField]
    GameObject weapon;
    [SerializeField]
    Transform head;
    //string[] stateNames = { "Base Layer.Idling", "Base Layer.Walking", "Base Layer.Sprinting",
    //    "Base Layer.Jumping", "Base Layer.Twisting", "Base Layer.Dashing",
    //    "Base Layer.NormalAttack1", "Base Layer.NormalAttack2", "Base Layer.NormalAttack3",
    //    "Base Layer.RisingWind", "Base Layer.RisingWater", "Base Layer.RisingFire",
    //    "Base Layer.SuperAttack1", "Base Layer.SuperAttack2" };
    //int[] stateHashes = new int[14];
    string[] attackStateNames = {
        "Base Layer.NormalAttack1", "Base Layer.NormalAttack2", "Base Layer.NormalAttack3",
        "Base Layer.RisingWind", "Base Layer.RisingWater", "Base Layer.RisingFire",
        "Base Layer.SuperAttack1", "Base Layer.SuperAttack2"
    };
    int[] attackStateNameHashes = new int[8];

    void Start()
    {
        animator = GetComponent<Animator>();
        //for (int i = 0; i < stateHashes.Length; i++)
        //{
        //    stateHashes[i] = Animator.StringToHash(stateNames[i]);
        //}
        AnimationContainer container = new WindAnimationContainer(animator);
        inputMoving = new PlayInput(container/*, stateHashes*/);
        for (int i = 0; i < attackStateNameHashes.Length; i++)
        {
            attackStateNameHashes[i] = Animator.StringToHash(attackStateNames[i]);
        }
        AnimatorStateMachine[] animatorStateMachineClones = animator.GetBehaviours<AnimatorStateMachine>();
        foreach (AnimatorStateMachine clone in animatorStateMachineClones)
        {
            clone.playerWeapon = weapon;
            clone.stateHashes = attackStateNameHashes;
        }
    }
    bool isOnGround;
    void Update()
    {
        inputMoving.SetAxisInputValue
            (Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        inputMoving.SetDirection(gameObject, head);
        inputMoving.ToWalk();
        inputMoving.ToJump(Input.GetKeyDown(KeyCode.Space), isOnGround);
        inputMoving.ToDash(Input.GetMouseButtonDown(1));
        inputMoving.ToSprint(Input.GetKey(KeyCode.LeftShift));
        inputMoving.ToTurnOnUniqueSkill(Input.GetKeyDown(KeyCode.Q));
        inputMoving.ToAnimateComboAttack(Input.GetMouseButtonDown(0));
        inputMoving.ToDoubleSuperAttack(Input.GetKeyDown(KeyCode.E));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isOnGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isOnGround = false;
    }
}
