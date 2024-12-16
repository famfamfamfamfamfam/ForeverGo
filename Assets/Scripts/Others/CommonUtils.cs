using System;
using UnityEngine;

public class CommonUtils
{
    static CommonUtils instance = new CommonUtils();
    public static CommonUtils Instance { get => instance; }
    private CommonUtils()
    {
        enumCount = Enum.GetValues(typeof(PowerKind)).Length;
    }

    public SelectedPowerKind playerPower { get; set; }
    public SelectedPowerKind monstersPower { get; set; }

    public bool onlyOneMode { get; set; }

    public int enumCount { get; private set; }

    public void SetUpNextValue(ref int currentValue, int numberOfCombo)
    {
        if (currentValue == numberOfCombo - 1)
            currentValue = 0;
        else
            currentValue++;
    }

    public PowerKind RandomMonsterKind()
    {
        int powerIndex = UnityEngine.Random.Range(0, 3);
        int unselectedKindIndex = powerIndex;
        SetUpNextValue(ref unselectedKindIndex, enumCount);
        return (PowerKind)powerIndex;
    }

    public void ToDealDamage(PowerKind damageTakerPower, PowerKind attackerPower, CharacterKind attacker, ref float ingredientHealth, float percentage)
    {
        DamageConfig damageData = RefToAssets.refs._damageDictionary[(attackerPower, attacker)];
        int bonusDamage = damageData.bonusDamageDictionary[damageTakerPower];
        if (attacker == CharacterKind.Player)
            GameManager.instance.Notify(TypeOfEvent.HasPlayerDamageDealt, (percentage, ingredientHealth, bonusDamage));
        ingredientHealth -= ResonanceDamage(ingredientHealth, percentage, bonusDamage);
    }

    public float GetPercentage(AttackState attackSate, PowerKind attackerPower, CharacterKind attacker)
    {
        DamageConfig damageData = RefToAssets.refs._damageDictionary[(attackerPower, attacker)];
        return damageData.percentageDamageDictionary[attackSate];
    }

    float NormalDamage(float ingredientHealth, float percentage)
    {
        return ingredientHealth * (percentage / 100);
    }

    float ResonanceDamage(float ingredientHealth, float percentage, int bonusDamage)
    {
        return NormalDamage(ingredientHealth, percentage) + bonusDamage;
    }

}
