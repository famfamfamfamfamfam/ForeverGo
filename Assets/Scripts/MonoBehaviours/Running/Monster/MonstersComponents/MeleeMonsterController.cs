using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeMonsterController : MonsterController
{
    Dictionary<int, Action<MonsterChip>> meeleeMonsterAttackDictionary;

    public int[] attackStateHashes { get; } = {
        Animator.StringToHash("Base Layer.SlapDownAttacking"),
        Animator.StringToHash("Base Layer.SwipeAttacking"),
        Animator.StringToHash("Base Layer.JumpAttacking")
    };

    private new void Awake()
    {
        base.Awake();
        meeleeMonsterAttackDictionary = new Dictionary<int, Action<MonsterChip>>()
        {
            { attackStateHashes[0], objChip => objChip._leftHand.enabled = !objChip._leftHand.enabled },
            { attackStateHashes[1], objChip => objChip._rightHand.enabled = !objChip._rightHand.enabled },
            { attackStateHashes[2], objChip =>
                {
                    objChip._leftHand.enabled = !objChip._leftHand.enabled;
                    objChip._rightHand.enabled = !objChip._rightHand.enabled;
                }
            }
        };
    }

    public void TurnDamagingTool(int stateHash, MonsterChip attackerChip)
    {
        meeleeMonsterAttackDictionary[stateHash].Invoke(attackerChip);
    }

    int jumpAttackTransitionHash = Animator.StringToHash("jumpAttack");
    int jumpAttackStateHash = Animator.StringToHash("Base Layer.JumpAttacking");
    public void ToJumpAttack()
    {
        container.TurnOnTemporaryAnimation(jumpAttackTransitionHash, jumpAttackStateHash);
    }


}
