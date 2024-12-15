using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    [SerializeField]
    GameObject body, distancePoint;
    [SerializeField]
    GameObject leftHand, rightHand;
    [SerializeField]
    GameObject HPBar;
    [SerializeField]
    Transform leftFoot;
    [SerializeField]
    Transform laserStartPoint;
    [SerializeField]
    CharacterProperties monsterProperties;
    [SerializeField]
    MeleeMonstersDefaultValues meleeMonstersDefaultValues;
    [SerializeField]
    RangedMonstersDefaultValues rangedMonstersDefaultValues;

    public Transform _laserStartPoint { get => laserStartPoint; }
    public Transform _leftFoot { get => leftFoot; }
    public CharacterProperties _monsterProperties { get => monsterProperties; }
    public MeleeMonstersDefaultValues _meleeMonstersDefaultValues { get => meleeMonstersDefaultValues; }
    public RangedMonstersDefaultValues _rangedMonstersDefaultValues { get => rangedMonstersDefaultValues; }
    public Collider _leftHand { get; private set; }
    public Collider _rightHand { get; private set; }
    public BoxCollider _distancePoint { get; private set; }
    public MaterialPropertyBlock HPMatProperty { get; private set; }
    public Renderer HPBarRenderer { get; private set; }
    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
        _leftHand = leftHand.GetComponent<Collider>();
        _rightHand = rightHand.GetComponent<Collider>();
        _distancePoint = distancePoint.GetComponent<BoxCollider>();
        HPBarRenderer = HPBar.GetComponent<Renderer>();
        HPMatProperty = new MaterialPropertyBlock();
        HPBarRenderer.GetPropertyBlock(HPMatProperty);
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
        {
            MonstersManager.instance.monsters.Remove(gameObject);
            MonsterController controller = GetComponent<MonsterController>();
            if (controller is RangedMonsterController rangedController)
                MonstersManager.instance._rangedMonsters.Remove(rangedController);
            else if (controller is MeleeMonsterController meleeController)
                MonstersManager.instance._meleeMonsters.Remove(meleeController);
        }
    }

}

[System.Serializable]
public class MeleeMonstersDefaultValues
{
    public DefaultValue healthPercentageToFlee;
    public DefaultValue startHandsAttackingDistance;
    public DefaultValue startFootAttackingDistance;
    public DefaultValue discoverPlayerDistance;
    public DefaultValue minValueForAdjustingDiscoverPlayerDistance;
    public DefaultValue maxValueForAdjustingDiscoverPlayerDistance;
    public DefaultValue changeRangeFrequency_countByFrame;
    public DefaultValue radiusOfTheRangeOfTrampleAttacking;
}

[System.Serializable]
public class RangedMonstersDefaultValues
{
    public DefaultValue laserWidth;
    public Vector3 colliderPositionRelativeToTheMonsterCenter;
    public Vector3 colliderSize;
    public DefaultValue roarFrequency_countBySecond;
    public DefaultValue laserLength;
}