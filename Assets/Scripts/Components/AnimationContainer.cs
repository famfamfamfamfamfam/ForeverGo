using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContainer
{
    protected Animator animCotroller;
    protected GameObject playerWeapon;
    public AnimationContainer(Animator animator, GameObject weapon)
    {
        animCotroller = animator;
        playerWeapon = weapon;
    }

    public void StartLoopAnimation(string transitionParamName)
    {
        if (!animCotroller.GetBool(transitionParamName))
            animCotroller.SetBool(transitionParamName, true);
    }

    public void StopLoopAnimation(string transitionParamName)
    {
        if (animCotroller.GetBool(transitionParamName))
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

    public void AnimateComboAttack()
    {
        playerWeapon.SetActive(true);
    }

    public virtual void UniqueSkill()
    {
        playerWeapon.SetActive(true);
    }

}
