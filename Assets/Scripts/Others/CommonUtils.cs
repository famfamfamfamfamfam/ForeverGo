using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtils
{
    static CommonUtils instance = new CommonUtils();
    public static CommonUtils Instance { get => instance; }
    private CommonUtils()
    {
        playerPower = new SelectedPowerKind();
        monstersPower = new SelectedPowerKind();

        selectedPowersDictionary = new Dictionary<CharacterKind, SelectedPowerKind>()
        {
            { CharacterKind.Player, playerPower },
            { CharacterKind.Monster, monstersPower },
        };

        enumCount = Enum.GetValues(typeof(PowerKind)).Length;
    }

    SelectedPowerKind playerPower;
    SelectedPowerKind monstersPower;

    Dictionary<CharacterKind, SelectedPowerKind> selectedPowersDictionary;

    public SelectedPowerKind GetSelectedPower(MonoBehaviour callingInstance, CharacterKind character)
    {
        if (callingInstance is MenuUIManager)
            return selectedPowersDictionary[character];
        SelectedPowerKind clone = new SelectedPowerKind();
        Array.Copy(selectedPowersDictionary[character].selectedPowerKinds, clone.selectedPowerKinds, selectedPowersDictionary[character].selectedPowerKinds.Length);
        return clone;
    }

    public bool onlyOneMode { get; private set; }

    public void SetOnlyOneMode(MonoBehaviour callingInstance, bool value)
    {
        if (callingInstance is MenuUIManager)
            onlyOneMode = value;
    }

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
