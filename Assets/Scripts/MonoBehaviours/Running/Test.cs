using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PowerData powerData;
    [SerializeField]
    NaturePowerKind powerKind;
    PlayInput inputMoving;
    Animator animator;
    Renderer playerRenderer;
    [SerializeField]
    GameObject weapon, body;
    [SerializeField]
    Transform head;
    string[] stateNames;
    int[] stateHashes;
    void Start()
    {
        stateNames = new string[14] { "Base Layer.Idling", "Base Layer.Walking", "Base Layer.Sprinting",
        "Base Layer.Jumping", "Base Layer.Twisting", "Base Layer.Dashing",
        "Base Layer.NormalAttack1", "Base Layer.NormalAttack2", "Base Layer.NormalAttack3",
        "Base Layer.RisingWind", "Base Layer.RisingWater", "Base Layer.RisingFire",
        "Base Layer.SuperAttack1", "Base Layer.SuperAttack2" };
        stateHashes = new int[14];

        playerRenderer = body.GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < stateHashes.Length; i++)
        {
            stateHashes[i] = Animator.StringToHash(stateNames[i]);
        }
        powerData = new PowerData(animator, stateHashes);
        AnimationContainer container = powerData.GetKindOfData(powerKind.powerKind).playerCurrentAnimContainer;
        playerRenderer.material = powerData.GetKindOfData(powerKind.powerKind).currentMaterial;
        inputMoving = new PlayInput(container, stateHashes);
        AnimatorStateMachine[] animatorStateMachineClones = animator.GetBehaviours<AnimatorStateMachine>();
        foreach (AnimatorStateMachine clone in animatorStateMachineClones)
        {
            clone.playerWeapon = weapon;
            clone.stateHashes = stateHashes;
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
        inputMoving.ToAnimateComboAttack(Input.GetMouseButtonDown(0), gameObject);
        inputMoving.ToDoubleSuperAttack(Input.GetKeyDown(KeyCode.E));
        inputMoving.ToChangeThePower(Input.GetKeyDown(KeyCode.F), powerData, ref powerKind.powerKind, powerKind.unselectedKind, playerRenderer);
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

    private void OnDestroy()
    {
        powerData.UnloadAssetsOnDestroy();
    }
}
