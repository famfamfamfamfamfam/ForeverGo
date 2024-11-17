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
    public void SetDirection(GameObject obj)
    {
        direction = new Vector3(horizontalValue, 0, verticalValue);
        if (direction != Vector3.zero)
            obj.transform.forward = direction;
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
}
