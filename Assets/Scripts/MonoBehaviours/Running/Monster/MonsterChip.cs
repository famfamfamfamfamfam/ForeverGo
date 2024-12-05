using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    [SerializeField]
    GameObject body;
    [SerializeField]
    GameObject leftHand, rightHand;
    [SerializeField]
    Transform laserStartPoint;
    [SerializeField]
    CharacterProperties monsterProperties;

    public Transform _laserStartPoint { get => laserStartPoint; }
    public CharacterProperties _monsterProperties { get => monsterProperties; }
    public Collider _leftHand { get; private set; }
    public Collider _rightHand { get; private set; }

    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
        _leftHand = leftHand.GetComponent<Collider>();
        _rightHand = rightHand.GetComponent<Collider>();
    }

    public void Init(PowerKind powerKind, Type monsterFightType, RuntimeAnimatorController monsterAnimatorController)
    {
        Renderer[] renderers = body.GetComponentsInChildren<Renderer>();
        MonsterPower powerProcessor = gameObject.AddComponent<MonsterPower>();
        powerProcessor.Init(powerKind, renderers);
        MonsterController controller = (MonsterController)gameObject.AddComponent(monsterFightType);
        controller.Init(monsterAnimatorController);
    }

    private void OnDisable()
    {
        if (MonstersManager.instance != null)
            MonstersManager.instance.monsters.Remove(gameObject);
    }

}
