using System;
using System.Collections.Generic;
using UnityEngine;

public class SwitchData
{
    Dictionary<PowerKind, PlayerData> data;
    public SwitchData(Animator animator, int[] stateHashes, float health, int hitCount)
    {
        data = new Dictionary<PowerKind, PlayerData>()
        {
            { PowerKind.Wind, new PlayerData(health, hitCount) },
            { PowerKind.Water, new PlayerData(health, hitCount) },
            { PowerKind.Fire, new PlayerData(health, hitCount) },
        };
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
    public float playerHealth { get; private set; }
    public int hitCountForUsingSkill { get; private set; }

    public PlayerData(float health, int hitCount)
    {
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


public class SelectedPowerKind
{
    public PowerKind[] selectedPowerKinds { get; private set; }
    public SelectedPowerKind()
    {
        selectedPowerKinds = new PowerKind[2];
    }
}

public class PlayerSelectionData
{
    static PlayerSelectionData instance;

    public static PlayerSelectionData Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerSelectionData();
            return instance;
        }
    }

    public static void EmptyTheInstance(MonoBehaviour callingInstance)
    {
        if (callingInstance is MenuUIManager && instance != null)
            instance = null;
    }

    private PlayerSelectionData()
    {
        playerPower = new SelectedPowerKind();
        monstersPower = new SelectedPowerKind();

        selectedPowersDictionary = new Dictionary<CharacterKind, SelectedPowerKind>()
        {
            { CharacterKind.Player, playerPower },
            { CharacterKind.Monster, monstersPower },
        };
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

}

public enum PowerKind
{
    Wind,
    Water,
    Fire
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