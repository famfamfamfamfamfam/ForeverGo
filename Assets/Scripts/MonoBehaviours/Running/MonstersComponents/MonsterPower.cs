using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPower : MonoBehaviour, IOnAttackable, IPowerKindGettable
{
    PowerKind powerKind;
    CharacterKind monsterChar = CharacterKind.Monster;

    float health;
    int resistanceToReact;
    int currentResistance;

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
        currentResistance = resistanceToReact;
    }

    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        currentResistance--;
        if (currentResistance == 0)
        {
            //animate react
            currentResistance = resistanceToReact;
        }
        // if enemyCurrentPower == null

        float percentage = CommonUtils.Instance.GetPercentage(enemyCurrentAttackState.Value, enemyCurrentPower, CharacterKind.Player);
        //add a method to change the below PowerKind
        CommonUtils.Instance.ToDealDamage(powerKind, enemyCurrentPower, CharacterKind.Player, ref health, percentage);
        //if (health <= 0)
            //animate die
    }

    public PowerKind GetPowerKind()
    {
        return powerKind;
    }
}
