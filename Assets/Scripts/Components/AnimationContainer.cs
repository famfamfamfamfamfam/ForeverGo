using UnityEngine;

public class AnimationContainer
{
    protected Animator animController;
    public AnimationContainer(Animator animator)
    {
        animController = animator;
    }

    public void StartLoopAnimation(int transitionHash)
    {
        if (!animController.GetBool(transitionHash))
            animController.SetBool(transitionHash, true);
    }

    public void StopLoopAnimation(int transitionHash)
    {
        if (animController.GetBool(transitionHash))
            animController.SetBool(transitionHash, false);
    }

    public void TurnOnTemporaryAnimation(int transitionHash, int stateHash)
    {
        if (!IsRunning(stateHash))
            animController.SetTrigger(transitionHash);
    }

    public void TurnOnSecondaryAnimation(int transitionHash, int stateHash, int previousStateHash)
    {
        if (IsRunning(previousStateHash))
            TurnOnTemporaryAnimation(transitionHash, stateHash);
    }

    int stateIndex, intParamValue;
    public void AnimateComboAttack(int transitionHash, int[] stateHashes)
    {
        if (!IsRunning(stateHashes[stateIndex]))
        {
            animController.SetInteger(transitionHash, intParamValue);
            CommonUtils.instance.SetUpNextValue(ref stateIndex, stateHashes.Length);
            intParamValue = stateIndex;
        }
    }

    public void ResetIntParam(int transitionHash, int firstValue)
    {
        animController.SetInteger(transitionHash, firstValue);
    }

    public bool IsRunning(int stateHash)
    {
        return animController.GetCurrentAnimatorStateInfo(0).fullPathHash == stateHash;
    }

    public virtual void UniqueSkill() { }

}
