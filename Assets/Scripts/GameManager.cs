using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    static GameManager instance = new GameManager();
    public static GameManager Instance { get => instance; }
    private GameManager()
    {
        enumCount = Enum.GetValues(typeof(PowerKind)).Length;
    }

    public int enumCount { get; private set; }

    public void SetUpNextValue(ref int currentValue, int numberOfCombo)
    {
        if (currentValue == numberOfCombo - 1)
            currentValue = 0;
        else
            currentValue++;
    }
}