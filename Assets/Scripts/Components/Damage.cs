using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage", menuName = "Damage")]
public class Damage : ScriptableObject
{
    public PowerKind powerKind;
    public CharacterKind character;
    public DamageData data;
}

[System.Serializable]
public class DamageData
{
    [SerializeField]
    float percentageDamageOnNormalAttack;
    [SerializeField]
    float percentageDamageOnSuperAttack;
    [SerializeField]
    float percentageDamageOnUniqueSkill;
    [SerializeField]
    int bonusDamageToWindCharacter;
    [SerializeField]
    int bonusDamageToWaterCharacter;
    [SerializeField]
    int bonusDamageToFireCharacter;

    public float _percentageDamageOnNormalAttack { get => percentageDamageOnNormalAttack; }
    public float _percentageDamageOnSuperAttack { get => percentageDamageOnSuperAttack; }
    public float _percentageDamageOnUniqueSkill { get => percentageDamageOnUniqueSkill; }
    public int _bonusDamageToWindCharacter { get => bonusDamageToWindCharacter; }
    public int _bonusDamageToWaterCharacter { get => bonusDamageToWaterCharacter; }
    public int _bonusDamageToFireCharacter { get => bonusDamageToFireCharacter; }
}