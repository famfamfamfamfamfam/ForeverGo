using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    [SerializeField]
    GameObject body, distancePoint;
    [SerializeField]
    GameObject leftHand, rightHand;
    [SerializeField]
    Transform leftFoot;
    [SerializeField]
    Transform laserStartPoint;
    [SerializeField]
    CharacterProperties monsterProperties;

    public Transform _laserStartPoint { get => laserStartPoint; }
    public Transform _leftFoot { get => leftFoot; }
    public CharacterProperties _monsterProperties { get => monsterProperties; }
    public Collider _leftHand { get; private set; }
    public Collider _rightHand { get; private set; }
    public BoxCollider _distancePoint { get; private set; }


    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
        _leftHand = leftHand.GetComponent<Collider>();
        _rightHand = rightHand.GetComponent<Collider>();
        _distancePoint = distancePoint.GetComponent<BoxCollider>();
    }

    public void Init(PowerKind powerKind, Type monsterFightType, RuntimeAnimatorController monsterAnimatorController)
    {
        Renderer[] renderers = body.GetComponentsInChildren<Renderer>();
        MonsterController controller = (MonsterController)gameObject.AddComponent(monsterFightType);
        controller.Init(monsterAnimatorController);
        MonsterPower powerProcessor = gameObject.AddComponent<MonsterPower>();
        powerProcessor.Init(powerKind, renderers);
    }

    private void OnDisable()
    {
        if (MonstersManager.instance != null)
            MonstersManager.instance.monsters.Remove(gameObject);
    }

}
