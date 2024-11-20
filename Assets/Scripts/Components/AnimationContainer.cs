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

    public void StartLoopAnimation(int transitionHash)
    {
        if (!animCotroller.GetBool(transitionHash))
            animCotroller.SetBool(transitionHash, true);
    }

    public void StopLoopAnimation(int transitionHash)
    {
        if (animCotroller.GetBool(transitionHash))
            animCotroller.SetBool(transitionHash, false);
    }

    public void TurnOnTemporaryAnimation(int transitionHash, int stateHash)
    {
        if (animCotroller.GetCurrentAnimatorStateInfo(0).fullPathHash != stateHash)
            animCotroller.SetTrigger(transitionHash);
    }

    public void TurnOnSecondaryAnimation(int transitionHash, int stateHash, int previousStateHash)
    {
        if (animCotroller.GetCurrentAnimatorStateInfo(0).fullPathHash == previousStateHash)
            TurnOnTemporaryAnimation(transitionHash, stateHash);
    }

    int stateIndex, intParamValue;
    public void AnimateComboAttack(int transitionHash, int[] stateHashes)
    {
        if (animCotroller.GetCurrentAnimatorStateInfo(0).fullPathHash != stateHashes[stateIndex])
        {
            animCotroller.SetInteger(transitionHash, intParamValue);
            SetUpNextValue(ref stateIndex, stateHashes.Length);
            intParamValue = stateIndex;
        }
    }

    void SetUpNextValue(ref int currentValue, int numberOfCombo)
    {
        if (currentValue == numberOfCombo - 1)
            currentValue = 0;
        else
            currentValue++;
    }

    public void ResetIntParam(int transitionHash, int firstValue)
    {
        animCotroller.SetInteger(transitionHash, firstValue);
    }

    public bool IsRunningState(int stateHash)
    {
        return animCotroller.GetCurrentAnimatorStateInfo(0).fullPathHash == stateHash;
    }

    public virtual void UniqueSkill() { }

}
