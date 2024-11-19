using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContainer
{
    protected Animator animCotroller;
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

    int stateNameIndex, intParamValue;
    public void AnimateComboAttack(string transitionParamName, string[] stateNames)
    {
        if (!animCotroller.GetCurrentAnimatorStateInfo(0).IsName(stateNames[stateNameIndex]))
        {
            animCotroller.SetInteger(transitionParamName, intParamValue);
            SetUpNextParamValues(stateNames.Length);
        }
    }

    void SetUpNextParamValues(int numberOfCombo)
    {
        if (stateNameIndex == numberOfCombo - 1)
            stateNameIndex = 0;
        else
            stateNameIndex++;
        intParamValue = stateNameIndex;
    }

    public void ResetIntParam(string transitionParamName)
    {
        animCotroller.SetInteger(transitionParamName, -1);
    }

    public virtual void UniqueSkill() { Debug.Log("ttt"); }

}
