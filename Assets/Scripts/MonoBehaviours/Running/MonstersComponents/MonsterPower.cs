using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPower : MonoBehaviour, IOnAttackable, IPowerKindGettable
{
    PowerKind powerKind;
    CharacterKind monsterChar = CharacterKind.Monster;

    float health;
    int resistanceToReact;

    public void Init(PowerKind kind, Renderer[] monsterRenderers)
    {
        powerKind = kind;
        foreach (Renderer renderer in monsterRenderers)
        {
            renderer.material = RefToAssets.refs._skinsDictionary[(kind, monsterChar)];
        }
    }

    private void Start()
    {
        CharacterProperties monsterProperties = GetComponent<MonsterChip>()._monsterProperties;
        health = monsterProperties.properties._health;
        resistanceToReact = monsterProperties.properties._resistanceToReact;
    }

    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        float percentage = CommonUtils.Instance.GetPercentage(enemyCurrentAttackState.Value, enemyCurrentPower, CharacterKind.Player);
        CommonUtils.Instance.ToDealDamage(powerKind, enemyCurrentPower, CharacterKind.Player, ref health, percentage);
    }

    public PowerKind GetPowerKind()
    {
        return powerKind;
    }
}
