using System;

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

    public float NormalDamage(int ingredientHealth, float percentage)
    {
        return ingredientHealth * percentage;
    }

    public float ResonanceDamage(int ingredientHealth, float percentage, int bonusDamage)
    {
        return NormalDamage(ingredientHealth, percentage) + bonusDamage;
    }

}