using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayInput
{
    AnimationContainer animationController;
    float horizontalValue, verticalValue;
    int[] stateHashes, subStateHashes;
    public PlayInput(AnimationContainer animationContainer, int[] stateHashes)
    {
        animationController = animationContainer;
        this.stateHashes = stateHashes;
        subStateHashes = new int[3] { stateHashes[6], stateHashes[7], stateHashes[8] };
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
                && (animationController.IsRunningState(stateHashes[1])
                || animationController.IsRunningState(stateHashes[2])))
            obj.transform.forward = objHead.rotation * direction;
    }

    int isWalking = Animator.StringToHash("isWalking");
    public void ToWalk()
    {
        if (direction != Vector3.zero)
            animationController.StartLoopAnimation(isWalking);
        else
            animationController.StopLoopAnimation(isWalking);
    }

    int jump = Animator.StringToHash("jump");
    int twist = Animator.StringToHash("twist");
    public void ToJump(bool hasInput, bool jumpCondition)
    {
        if (hasInput)
        {
            if (jumpCondition)
                animationController.TurnOnTemporaryAnimation(jump, stateHashes[3]);
            animationController.TurnOnSecondaryAnimation(twist, stateHashes[4], stateHashes[3]);
        }
    }

    int dash = Animator.StringToHash("dash");
    public void ToDash(bool hasInput)
    {
        if (hasInput)
            animationController.TurnOnTemporaryAnimation(dash, stateHashes[5]);
    }

    int isSprinting = Animator.StringToHash("isSprinting");
    public void ToSprint(bool hasInput)
    {
        if (hasInput)
            animationController.StartLoopAnimation(isSprinting);
        else
            animationController.StopLoopAnimation(isSprinting);
    }

    public void ToTurnOnUniqueSkill(bool turnOnUniqueSkillCondition)
    {
        if (turnOnUniqueSkillCondition)
            animationController.UniqueSkill();
    }

    int nAttack = Animator.StringToHash("nAttack");
    public void ToAnimateComboAttack(bool hasInput, GameObject obj)
    {
        if (hasInput)
        {
            LookForwardAMonster(obj);
            animationController.AnimateComboAttack(nAttack, subStateHashes);
        }
        else
        {
            animationController.ResetIntParam(nAttack, -1);
        }
    }

    Collider[] monsterTargets = new Collider[1];
    int monsLayerMask = LayerMask.GetMask("Monster");
    Vector3 directionToMonster;
    void LookForwardAMonster(GameObject obj)
    {
        if (Physics.OverlapSphereNonAlloc(obj.transform.position, 2f, monsterTargets, monsLayerMask) > 0)
        {
            directionToMonster = monsterTargets[0].gameObject.transform.position - obj.transform.position;
            directionToMonster.y = obj.transform.position.y;
            obj.transform.forward = directionToMonster.normalized;
        }
    }


    int sAttack = Animator.StringToHash("sAttack");
    int sAttackx2 = Animator.StringToHash("sAttackx2");
    public void ToDoubleSuperAttack(bool hasInput)
    {
        if (hasInput)
        {
            animationController.TurnOnTemporaryAnimation(sAttack, stateHashes[12]);
            animationController.TurnOnSecondaryAnimation(sAttackx2, stateHashes[13], stateHashes[12]);
        }
    }

    int enumCount = Enum.GetValues(typeof(PlayerPowerKind)).Length;
    public void ToChangeThePower(bool hasInput, PowerData powerData, ref PlayerPowerKind currentPowerKind, PlayerPowerKind unselectedKind, Renderer renderer)
    {
        if(hasInput)
        {
            int currentPowerKindIndex = (int)currentPowerKind;
            do
            {
                animationController.SetUpNextValue(ref currentPowerKindIndex, enumCount);
                currentPowerKind = (PlayerPowerKind)currentPowerKindIndex;
            } while (currentPowerKind == unselectedKind);
            PlayerData playerData = powerData.GetKindOfData(currentPowerKind);
            animationController = playerData.playerCurrentAnimContainer;
            renderer.material = playerData.currentMaterial;
        }
    }
}
