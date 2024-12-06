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

    private void OnEnable()
    {
        CharacterProperties monsterProperties = GetComponent<MonsterChip>()._monsterProperties;
        health = monsterProperties.properties._health;
        resistanceToReact = monsterProperties.properties._resistanceToReact;
        currentResistance = resistanceToReact;
    }

    MonsterController theMonster;
    RangedMonsterController rangedMonster;
    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        theMonster = gameObject.GetComponent<MonsterController>();
        currentResistance--;
        if (currentResistance == 0)
        {
            theMonster.ToReact();
            currentResistance = resistanceToReact;
        }

        rangedMonster = gameObject.GetComponent<RangedMonsterController>();
        if (rangedMonster != null && MonstersManager.instance.rangedMonstersHitTakableCount > 0)
        {
            MonstersManager.instance.rangedMonstersHitTakableCount--;
            if (MonstersManager.instance.rangedMonstersHitTakableCount == 0)
                rangedMonster.ToScream(MonstersManager.instance._player.transform.position);
        }

        float percentage = CommonUtils.Instance.GetPercentage(enemyCurrentAttackState.Value, enemyCurrentPower, CharacterKind.Player);
        //need to add a method that changes the below PowerKind of enemy to minus the resonance damage
        CommonUtils.Instance.ToDealDamage(powerKind, enemyCurrentPower, CharacterKind.Player, ref health, percentage);
        if (health <= 0)
        {
            theMonster.ToDie();
        }
        Debug.Log(health);
    }

    public PowerKind GetPowerKind()
    {
        return powerKind;
    }
}
