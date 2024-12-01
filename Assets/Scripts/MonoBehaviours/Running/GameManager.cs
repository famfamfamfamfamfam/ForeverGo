using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool gameOver { get; private set; }
    public bool gamePause { get; private set; }

    public void OnAttack(GameObject attacker, GameObject damageTaker)
    {
        IPowerKindGettable iAttacker = attacker.GetComponent<IPowerKindGettable>();
        if (iAttacker == null)
            return;
        PowerKind attackerPowerKind = iAttacker.GetPowerKind();
        IAttackStateGettable iAttack = attacker.GetComponent<IAttackStateGettable>();
        AttackState? attackerAttackState = iAttack?.GetAttackState();
        damageTaker.GetComponent<IOnAttackable>()?.OnBeAttacked(attackerPowerKind, attackerAttackState);
    }

    public void SetCurrentAttackState(AttackState? newAttackState, GameObject settableObj)
    {
        settableObj.GetComponent<IAttackStateSettable>()?.SetAttackState(newAttackState);
    }
}
