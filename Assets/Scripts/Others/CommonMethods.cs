using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class CommonMethods
{
    static CommonMethods instance = new CommonMethods();
    public static CommonMethods Instance { get => instance; }
    private CommonMethods()
    {
        enumCount = Enum.GetValues(typeof(PowerKind)).Length;
    }

    public bool onlyOneMode;

    public int enumCount { get; private set; }

    public void SetUpNextValue(ref int currentValue, int numberOfCombo)
    {
        if (currentValue == numberOfCombo - 1)
            currentValue = 0;
        else
            currentValue++;
    }

    public PowerKind RandomMonsterKind(ref PowerKind unselectedKind)
    {
        int powerIndex = UnityEngine.Random.Range(0, 3);
        int unselectedKindIndex = powerIndex;
        SetUpNextValue(ref unselectedKindIndex, enumCount);
        unselectedKind = (PowerKind)unselectedKindIndex;
        return (PowerKind)powerIndex;
    }

    public void ToDealDamage(PowerKind damageTakerPower, PowerKind attackerPower, CharacterKind attacker, ref float ingredientHealth, float percentage)
    {
        int bonusDamage = 0;
        DamageData damageData = RefToAssets.refs._damageDictionary[(attackerPower, attacker)];
        bonusDamage = damageData.bonusDamageDictionary[damageTakerPower];
        ingredientHealth -= ResonanceDamage(ingredientHealth, percentage, bonusDamage);
    }

    public float GetPercentage(AttackState attackSate, PowerKind attackerPower, CharacterKind attacker)
    {
        DamageData damageData = RefToAssets.refs._damageDictionary[(attackerPower, attacker)];
        return damageData.percentageDamageDictionary[attackSate];
    }

    float NormalDamage(float ingredientHealth, float percentage)
    {
        return ingredientHealth * percentage;
    }

    float ResonanceDamage(float ingredientHealth, float percentage, int bonusDamage)
    {
        return NormalDamage(ingredientHealth, percentage) + bonusDamage;
    }

}
