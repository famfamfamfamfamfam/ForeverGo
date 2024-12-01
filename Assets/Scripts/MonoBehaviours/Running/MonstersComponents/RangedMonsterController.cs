using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonsterController : MonsterController
{
    [SerializeField]
    RuntimeAnimatorController animatorController;

    private void Awake()
    {
        animator.runtimeAnimatorController = animatorController;
    }

}
