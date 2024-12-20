using System.Collections.Generic;
using System;
using UnityEngine;

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