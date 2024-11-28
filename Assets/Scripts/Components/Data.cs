using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchData
{
    Dictionary<PowerKind, PlayerData> data;
    public SwitchData(Animator animator, int[] stateHashes, PowerKind unselectedKind, float health)
    {
        data = new Dictionary<PowerKind, PlayerData>()
        {
            { PowerKind.Wind, new PlayerData(new WindAnimationContainer(animator, stateHashes[9]), health) },
            { PowerKind.Water, new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]), health) },
            { PowerKind.Fire, new PlayerData(new FireAnimationContainer(animator, stateHashes[11]), health) },
        };
        data.Remove(unselectedKind);
        RefToAssets.refs._skinsDictionary.Remove((unselectedKind, CharacterKind.Player));
    }

    public AnimationContainer GetYourAnimationContainer(PowerKind powerKind)
    {
        return data[powerKind].animContainer;
    }

    public float GetHealth(PowerKind powerKind)
    {
        return data[powerKind].playerHealth;
    }

    public void SetHealth(PowerKind powerKind, float currentHealth)
    {
        data[powerKind].SetPlayerHealth(currentHealth);
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
public class GeneralData
{
    [SerializeField]
    float health;
    [SerializeField]
    int resistanceToReact;

    public float _health { get => health; }
    public int _resistanceToReact { get => resistanceToReact; }
}