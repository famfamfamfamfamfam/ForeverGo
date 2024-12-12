using UnityEngine;

public class MonsterPower : MonoBehaviour, IOnAttackable, IPowerKindGettable
{
    PowerKind powerKind;
    CharacterKind monsterChar = CharacterKind.Monster;

    float defaultHealth, health;
    int resistanceToReact;
    int currentResistance;

    MonsterChip chip;
    public void Init(PowerKind kind, Renderer[] monsterRenderers)
    {
        powerKind = kind;
        foreach (Renderer renderer in monsterRenderers)
        {
            renderer.material = RefToAssets.refs._skinsDictionary[(kind, monsterChar)];
        }
        theMonster = gameObject.GetComponent<MonsterController>();
        chip = gameObject.GetComponent<MonsterChip>();
    }

    private void OnEnable()
    {
        CharacterProperties monsterProperties = GetComponent<MonsterChip>()._monsterProperties;
        defaultHealth = monsterProperties.properties._health;
        health = defaultHealth;
        resistanceToReact = monsterProperties.properties._resistanceToReact;
        currentResistance = resistanceToReact;
    }

    MonsterController theMonster;
    public void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState)
    {
        currentResistance--;
        if (currentResistance == 0)
        {
            theMonster.ToReact();
            currentResistance = resistanceToReact;
        }

        if (theMonster is RangedMonsterController)
            MonstersManager.instance.DiscoverPlayerOnRangedMonsters();

        float percentage = CommonUtils.Instance.GetPercentage(enemyCurrentAttackState.Value, enemyCurrentPower, CharacterKind.Player);
        CommonUtils.Instance.ToDealDamage(powerKind, enemyCurrentPower, CharacterKind.Player, ref health, percentage);
        if (health <= 0)
            theMonster.ToDie();

        if (theMonster is MeleeMonsterController meleeMonster)
        {
            if (health < defaultHealth * chip._meleeMonstersDefaultValues.healthPercentageToFlee.value / 100)
                meleeMonster.ToFleeOnLowHP();
        }
        Debug.Log(health);
    }

    public PowerKind GetPowerKind()
    {
        return powerKind;
    }
}
