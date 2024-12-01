using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimatorController(RuntimeAnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }
}
