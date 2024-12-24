using UnityEngine;

public class MonsterPower : MonoBehaviour, IOnAttackable, IGetForAttacking
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
            renderer.sharedMaterial = CommonConfig.instance._skinsDictionary[(kind, monsterChar)];
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
        if (health <= 0)
            return;

        currentResistance--;
        if (currentResistance == 0)
        {
            theMonster.ToReact();
            currentResistance = resistanceToReact;
        }

        if (theMonster is RangedMonsterController rangedMonster)
            rangedMonster.ToDiscoverPlayer();

        float percentage = CommonUtils.instance.GetPercentage(enemyCurrentAttackState.Value, enemyCurrentPower, CharacterKind.Player);
        CommonUtils.instance.ToDealDamage(powerKind, enemyCurrentPower, CharacterKind.Player, ref health, percentage);
        
        GameManager.instance.Notify(TypeOfEvent.MonstersHPChange, (chip._renderer, chip.HPBarProperties, health));
        
        if (health <= 0)
            theMonster.ToDie();

        if (theMonster is MeleeMonsterController meleeMonster)
        {
            if (health < defaultHealth * chip._meleeMonstersDefaultValues.healthPercentageToFlee.value / 100)
                meleeMonster.ToFleeOnLowHP();
        }
        Debug.Log(health);
    }

    public (PowerKind powerKind, AttackState? attackState) GetDataForAttacking()
    {
        return (powerKind, null);
    }
}
