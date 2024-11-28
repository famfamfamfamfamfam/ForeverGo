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

    public void ToDealResonanceDamage(PowerKind powerKind, CharacterKind character, int ingredientHealth, float percentage)
    {
        int bonusDamage = 0;
        DamageData damageData = RefToAssets.refs._damageDictionary[(powerKind, character)];
        switch (powerKind)
        {
            case PowerKind.Wind:
                bonusDamage = damageData._bonusDamageToWindCharacter;
                break;
            case PowerKind.Water:
                bonusDamage = damageData._bonusDamageToWaterCharacter;
                break;
            case PowerKind.Fire:
                bonusDamage = damageData._bonusDamageToFireCharacter;
                break;
        }
        ResonanceDamage(ingredientHealth, percentage, bonusDamage);
    }

    public float NormalDamage(int ingredientHealth, float percentage)
    {
        return ingredientHealth * percentage;
    }

    public float ResonanceDamage(int ingredientHealth, float percentage, int bonusDamage)
    {
        return NormalDamage(ingredientHealth, percentage) + bonusDamage;
    }

}
