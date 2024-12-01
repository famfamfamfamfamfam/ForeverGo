using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    protected Animator animator;
    protected AnimationContainer container;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init(RuntimeAnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
        container = new AnimationContainer(animator);
    }
}
