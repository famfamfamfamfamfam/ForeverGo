using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayInput
{
    AnimationContainer animationController;
    float horizontalValue, verticalValue;
    public PlayInput(AnimationContainer animationContainer)
    {
        animationController = animationContainer;
    }

    public void SetAxisInputValue(float horizontalInput, float verticalInput)
    {
        horizontalValue = horizontalInput;
        verticalValue = verticalInput;
    }

    Vector3 direction;
    public void SetDirection(GameObject obj, Transform objHead)
    {
        direction = new Vector3(horizontalValue, 0, verticalValue);
        if (direction != Vector3.zero
                && (animationController.IsRunningState("Walking")
                || animationController.IsRunningState("Sprinting")))
            obj.transform.forward = objHead.rotation * direction;
    }

    public void ToWalk()
    {
        if (direction != Vector3.zero)
            animationController.StartLoopAnimation("isWalking");
        else
            animationController.StopLoopAnimation("isWalking");
    }

    public void ToJump(bool hasInput, bool jumpCondition)
    {
        if (hasInput)
        {
            if (jumpCondition)
                animationController.TurnOnTemporaryAnimation("jump", "Jumping");
            animationController.TurnOnSecondaryAnimation("twist", "Twisting", "Jumping");
        }
    }

    public void ToDash(bool hasInput)
    {
        if (hasInput)
            animationController.TurnOnTemporaryAnimation("dash", "Dashing");
    }

    public void ToSprint(bool hasInput)
    {
        if (hasInput)
            animationController.StartLoopAnimation("isSprinting");
        else
            animationController.StopLoopAnimation("isSprinting");
    }

    public void ToTurnOnUniqueSkill(bool turnOnUniqueSkillCondition)
    {
        if (turnOnUniqueSkillCondition)
            animationController.UniqueSkill();
    }

    string[] stateNames = { "NormalAttack1", "NormalAttack2", "NormalAttack3" };
    public void ToAnimateComboAttack(bool hasInput)
    {
        if (hasInput)
            animationController.AnimateComboAttack("nAttack", stateNames);
        else
            animationController.ResetIntParam("nAttack", -1);
    }

    public void ToDoubleSuperAttack(bool hasInput)
    {
        if (hasInput)
        {
            animationController.TurnOnTemporaryAnimation("sAttack", "SuperAttack1");
            animationController.TurnOnSecondaryAnimation("sAttackx2", "SuperAttack2", "SuperAttack1");
        }
    }
}
