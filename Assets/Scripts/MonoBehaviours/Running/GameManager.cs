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
    public bool gameOver { get; set; }
    public bool gamePause { get; set; }

    public void OnAttack(GameObject attacker, GameObject damageTaker)
    {
        if (!gamePause && !gameOver)
        {
            IPowerKindGettable iAttacker = attacker.GetComponent<IPowerKindGettable>();
            if (iAttacker == null)
                return;
            PowerKind attackerPowerKind = iAttacker.GetPowerKind();
            IAttackStateGettable iAttack = attacker.GetComponent<IAttackStateGettable>();
            AttackState? attackerAttackState = iAttack?.GetAttackState();
            damageTaker.GetComponent<IOnAttackable>()?.OnBeAttacked(attackerPowerKind, attackerAttackState);
            attacker.GetComponent<IHitCountForUsingSkillSettable>()?.SetHitCount();
        }
    }

    public void SetCurrentAttackState(AttackState? newAttackState, GameObject settableObj)
    {
        settableObj.GetComponent<IAttackStateSettable>()?.SetAttackState(newAttackState);
    }
}
