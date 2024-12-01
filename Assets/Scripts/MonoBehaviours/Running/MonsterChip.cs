using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    MonsterPower powerProcessor;
    Type controllerType;

    PowerKind? powerKind;
    Renderer[] renderers;

    public void SetPowerKind(PowerKind newKind)
    {
        powerKind = newKind;
    }

    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
    }

    public void Init()
    {
        renderers = GetComponentsInChildren<Renderer>();
        powerProcessor = gameObject.AddComponent<MonsterPower>();
        powerProcessor.Init(powerKind.Value, renderers);
        gameObject.AddComponent(controllerType);
    }
}
