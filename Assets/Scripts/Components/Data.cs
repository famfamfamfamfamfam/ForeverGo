using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwitchData
{
    Dictionary<PowerKind, PlayerData> data;
    public SwitchData(Animator animator, int[] stateHashes, float health, int hitCount)
    {
        data = new Dictionary<PowerKind, PlayerData>()
        {
            { PowerKind.Wind, new PlayerData(new WindAnimationContainer(animator, stateHashes[9]), health, hitCount) },
            { PowerKind.Water, new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]), health, hitCount) },
            { PowerKind.Fire, new PlayerData(new FireAnimationContainer(animator, stateHashes[11]), health, hitCount) },
        };
    }

    public AnimationContainer GetYourAnimationContainer(PowerKind powerKind)
    {
        return data[powerKind].animContainer;
    }

    public float GetHealth(PowerKind powerKind)
    {
        float currentHealth = data[powerKind].playerHealth;
        GameManager.instance.Notify(TypeOfEvent.PlayerHPChange, currentHealth);
        return currentHealth;
    }

    public void SetHealth(PowerKind powerKind, float currentHealth)
    {
        data[powerKind].SetPlayerHealth(currentHealth);
        GameManager.instance.Notify(TypeOfEvent.PlayerHPChange, currentHealth);
    }

    public int GetHitCount(PowerKind powerKind)
    {
        int currentHitCount = data[powerKind].hitCountForUsingSkill;
        GameManager.instance.Notify(TypeOfEvent.PlayerUniqueSkillStatusChange, currentHitCount);
        return currentHitCount;
    }

    public void SetHitCount(PowerKind powerKind, int newNumber)
    {
        data[powerKind].SetHitCountForUsingSkill(newNumber);
        GameManager.instance.Notify(TypeOfEvent.PlayerUniqueSkillStatusChange, newNumber);
    }
}

public class PlayerData
{
    public AnimationContainer animContainer { get; private set; }
    public float playerHealth { get; private set; }
    public int hitCountForUsingSkill { get; private set; }

    public PlayerData(AnimationContainer animationContainer, float health, int hitCount)
    {
        animContainer = animationContainer;
        playerHealth = health;
        hitCountForUsingSkill = hitCount;
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerHealth = newHealth;
    }

    public void SetHitCountForUsingSkill(int newNumber)
    {
        hitCountForUsingSkill = newNumber;
    }
}

[System.Serializable]
public class GeneralConfig
{
    [SerializeField]
    float health;
    [SerializeField]
    int resistanceToReact;

    public float _health { get => health; }
    public int _resistanceToReact { get => resistanceToReact; }
}