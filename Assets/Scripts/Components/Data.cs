using System.Collections.Generic;
using UnityEngine;

public class SwitchData
{
    Dictionary<PowerKind, PlayerData> data;
    public SwitchData(Animator animator, int[] stateHashes, float health)
    {
        data = new Dictionary<PowerKind, PlayerData>()
        {
            { PowerKind.Wind, new PlayerData(new WindAnimationContainer(animator, stateHashes[9]), health) },
            { PowerKind.Water, new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]), health) },
            { PowerKind.Fire, new PlayerData(new FireAnimationContainer(animator, stateHashes[11]), health) },
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

}

public class PlayerData
{
    public AnimationContainer animContainer { get; private set; }
    public float playerHealth { get; private set; }

    public PlayerData(AnimationContainer animationContainer, float health)
    {
        animContainer = animationContainer;
        playerHealth = health;
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerHealth = newHealth;
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