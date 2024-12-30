using System.Resources;
using UnityEngine;

public class BossPower : MonoBehaviour, IGetForAttacking, IOnAttackable
{
    public (PowerKind powerKind, AttackState? attackState) GetDataForAttacking()
    {
        return ((PowerKind)Random.Range(0, CommonUtils.instance.powerKindEnumCount), null);
    }

    PowerKind mark;
    bool canTakeDamage;

    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {

    }

    public void SetTakeDamageState(bool state)
    {
        canTakeDamage = state;
    }

}
