using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksEffect
{
    GameObject theAttacker;
    public AttacksEffect(GameObject attacker)
    {
        theAttacker = attacker;
    }

    public void AfterSuperAttack()
    {
        //theAttacker.GetComponent<IAttackStateSettable>()?.SetAttackState();
    }
}
