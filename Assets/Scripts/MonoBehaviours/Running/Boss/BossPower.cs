using System.Resources;
using UnityEngine;

public class BossPower : MonoBehaviour, IGetForAttacking
{
    public (PowerKind powerKind, AttackState? attackState) GetDataForAttacking()
    {
        return ((PowerKind)Random.Range(0, CommonUtils.instance.powerKindEnumCount), null);
    }

    PowerKind mark;
    bool canTakeDamage;

    void Start()
    {
        
    }

    public void SetTakeDamageState(bool state)
    {
        canTakeDamage = state;
    }
}
