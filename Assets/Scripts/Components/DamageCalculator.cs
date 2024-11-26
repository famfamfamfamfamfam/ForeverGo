using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    protected float NormalDamage(int ingredientHealth, float percentage)
    {
        return ingredientHealth * percentage;
    }

    protected float ResonanceDamage(int ingredientHealth, float percentage, int bonusDamage)
    {
        return NormalDamage(ingredientHealth, percentage) + bonusDamage;
    }
}