using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage", menuName = "Damage")]
public class Damage : ScriptableObject
{
    public PowerKind powerKind;
    public CharacterKind character;
    public DamageConfig data;
}

[System.Serializable]
public class DamageConfig
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

    public Dictionary<PowerKind, int> bonusDamageDictionary { get; private set; }
    public Dictionary<AttackState, float> percentageDamageDictionary { get; private set; }
    public void InitBonusDamageDictionary()
    {
        bonusDamageDictionary = new Dictionary<PowerKind, int>()
        {
            { PowerKind.Wind, bonusDamageToWindCharacter},
            { PowerKind.Water, bonusDamageToWaterCharacter},
            { PowerKind.Fire, bonusDamageToFireCharacter},
        };
    }
    public void InitPercentageDamageDictionary()
    {
        percentageDamageDictionary = new Dictionary<AttackState, float>()
        {
            { AttackState.NormalAttack, percentageDamageOnNormalAttack },
            { AttackState.SuperAttack, percentageDamageOnSuperAttack },
            { AttackState.UniqueSkill, percentageDamageOnUniqueSkill },
        };
    }
}