using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    [SerializeField]
    GameObject body;

    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
    }

    public void Init(PowerKind powerKind, Type monsterFightType, RuntimeAnimatorController monsterAnimatorController)
    {
        Renderer[] renderers = body.GetComponentsInChildren<Renderer>();
        MonsterPower powerProcessor = gameObject.AddComponent<MonsterPower>();
        powerProcessor.Init(powerKind, renderers);
        MonsterController controller = (MonsterController)gameObject.AddComponent(monsterFightType);
        controller.SetAnimatorController(monsterAnimatorController);
    }
}
