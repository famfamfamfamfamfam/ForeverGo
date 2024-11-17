using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PlayInput inputMoving;
    Animator animator;
    Rigidbody rb;
    [SerializeField]
    GameObject weapon;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        AnimationContainer container = new WindAnimationContainer(animator, weapon);
        inputMoving = new PlayInput(container);
    }
    bool isOnGround;
    void Update()
    {
        inputMoving.SetAxisInputValue
            (Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        inputMoving.SetDirection(gameObject);
        inputMoving.ToWalk();
        inputMoving.ToJump(Input.GetKeyDown(KeyCode.Space), isOnGround);
        inputMoving.ToDash(Input.GetMouseButtonDown(1));
        inputMoving.ToSprint(Input.GetKey(KeyCode.LeftShift));
        inputMoving.ToTurnOnUniqueSkill(Input.GetKeyDown(KeyCode.Q));
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
