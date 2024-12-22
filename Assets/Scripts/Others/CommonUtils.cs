using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtils
{
    public static CommonUtils instance = new CommonUtils();
    private CommonUtils()
    {
        enumCount = Enum.GetValues(typeof(PowerKind)).Length;
    }

    public int enumCount { get; private set; }

    public void SetUpNextIndex(ref int currentValue, int collectionSize)
    {
        if (currentValue == collectionSize - 1)
            currentValue = 0;
        else
            currentValue++;
    }

    public PowerKind RandomMonsterKind()
    {
        int powerIndex = UnityEngine.Random.Range(0, 3);
        int unselectedKindIndex = powerIndex;
        SetUpNextIndex(ref unselectedKindIndex, enumCount);
        return (PowerKind)powerIndex;
    }

    public void ToDealDamage(PowerKind damageTakerPower, PowerKind attackerPower, CharacterKind attacker, ref float ingredientHealth, float percentage)
    {
        DamageConfig damageData = CommonConfig.instance._damageDictionary[(attackerPower, attacker)];
        int bonusDamage = damageData.bonusDamageDictionary[damageTakerPower];
        if (attacker == CharacterKind.Player)
            GameManager.instance.Notify(TypeOfEvent.HasPlayerDamageDealt, (percentage, ingredientHealth, bonusDamage));
        ingredientHealth -= ResonanceDamage(ingredientHealth, percentage, bonusDamage);
    }

    public float GetPercentage(AttackState attackSate, PowerKind attackerPower, CharacterKind attacker)
    {
        DamageConfig damageData = CommonConfig.instance._damageDictionary[(attackerPower, attacker)];
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