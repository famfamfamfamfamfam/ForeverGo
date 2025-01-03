﻿using System.Collections;
using UnityEngine;

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
        if (horizontalInput != 0)
            GameManager.instance._playerFollowCam.SetSmoothTime(0);
        else if (GameManager.instance._playerFollowCam.SmoothTimeIsZeroCurrently())
            GameManager.instance._playerFollowCam.ResetSmoothTimeToDefault();

        horizontalValue = horizontalInput;
        verticalValue = verticalInput;
    }

    Vector3 direction;
    public void SetDirection(GameObject obj, Transform objHead)
    {
        direction = new Vector3(horizontalValue, 0, verticalValue);
        if (direction != Vector3.zero)
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

    public void ToTurnOnUniqueSkill(PowerKind currentPower, SwitchData data, bool turnOnUniqueSkillCondition, ref int needResetVairable)
    {
        if (turnOnUniqueSkillCondition)
        {
            animationController.UniqueSkill();
            needResetVairable = 0;
            data.SetHitCount(currentPower, needResetVairable);
        }
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
    public void ToTurnOnSuperAttack(bool hasInput, ref bool isInCooldown)
    {
        if (hasInput && !isInCooldown)
        {
            animationController.TurnOnTemporaryAnimation(sAttack, stateHashes[12]);
            isInCooldown = true;
        }
    }

    public void ToChangeThePower(bool hasInput, ref PowerKind currentPowerKind, PowerKind[] powerKinds, CharacterKind character, ref float health, ref int hitCount, SwitchData playerData, Renderer renderer)
    {
        if(hasInput)
        {
            ProcessSwitching(ref currentPowerKind, powerKinds);
            SetUpStateAfterSwitch(currentPowerKind, playerData, ref health, ref hitCount, renderer, character);
        }
    }

    int currentPowerKindIndex = 0;
    void ProcessSwitching(ref PowerKind currentPowerKind, PowerKind[] powerKinds)
    {
        CommonUtils.instance.SetUpNextIndex(ref currentPowerKindIndex, powerKinds.Length);
        currentPowerKind = powerKinds[currentPowerKindIndex];
    }
    void SetUpStateAfterSwitch(PowerKind currentPowerKind, SwitchData playerData, ref float health, ref int hitCount, Renderer renderer, CharacterKind character)
    {
        animationController = CommonConfig.instance.playerAnimationContainer[currentPowerKind];
        health = playerData.GetHealth(currentPowerKind);
        hitCount = playerData.GetHitCount(currentPowerKind);
        renderer.sharedMaterial = CommonConfig.instance._skinsDictionary[(currentPowerKind, character)];
    }
}
