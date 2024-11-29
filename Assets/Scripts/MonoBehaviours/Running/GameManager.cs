using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    List<ScriptableObject> dataSavingThroughScenes;

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
        damageTaker.GetComponent<IOnAttackable>()?.OnBeAttacked(attackerPowerKind);
    }

    public void SetCurrentAttackState(AttackState? newAttackState, GameObject settableObj)
    {
        settableObj.GetComponent<IAttackStateSettable>()?.SetAttackState(newAttackState);
    }
}
