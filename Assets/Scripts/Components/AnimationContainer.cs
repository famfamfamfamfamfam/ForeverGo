using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContainer
{
    Animator animCotroller;
    public AnimationContainer(Animator animator)
    {
        animCotroller = animator;
    }

    public void StartLoopAnimation(string transitionParamName)
    {
        if (!animCotroller.GetBool(transitionParamName))
            animCotroller.SetBool(transitionParamName, true);
    }

    public void StopLoopAnimation(string transitionParamName)
    {
        animCotroller.SetBool(transitionParamName, false);
    }

    public void TurnOnTemporaryAnimation(string transitionParamName, string stateName)
    {
        if (!animCotroller.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            animCotroller.SetTrigger(transitionParamName);
    }

    public void TurnOnSecondaryAnimation(string transitionParamName, string stateName, string previousStateName)
    {
        if (animCotroller.GetCurrentAnimatorStateInfo(0).IsName(previousStateName))
            TurnOnTemporaryAnimation(transitionParamName, stateName);
    }
}
