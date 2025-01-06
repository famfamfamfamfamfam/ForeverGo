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
        animator.SetBool("isDying", false);
    }

    int reactTransitionHash = Animator.StringToHash("react");
    int reactStateHash = Animator.StringToHash("Base Layer.Reacting");
    public void ToReact()
    {
        container.TurnOnTemporaryAnimation(reactTransitionHash, reactStateHash);
    }

    int dieTransitionHash = Animator.StringToHash("die");
    int dieStateHash = Animator.StringToHash("Base Layer.Dying");
    public void ToDie()
    {
        container.TurnOnTemporaryAnimation(dieTransitionHash, dieStateHash);
    }
}
