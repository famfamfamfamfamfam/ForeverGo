using System;
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
    public Renderer _renderer { get; private set; }
    public MaterialPropertyBlock HPBarProperties { get; private set; }
    void OnEnable()
    {
        _leftHand = leftHand.GetComponent<Collider>();
        _rightHand = rightHand.GetComponent<Collider>();
        _distancePoint = distancePoint.GetComponent<BoxCollider>();
        _renderer = HPBar.GetComponent<Renderer>();
        HPBarProperties = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(HPBarProperties);
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
            MonstersManager.instance.RemoveMonsterFromList(gameObject);
    }

}

[Serializable]
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

[Serializable]
public class RangedMonstersDefaultValues
{
    public DefaultValue laserWidth;
    public Vector3 colliderPositionRelativeToTheMonsterCenter;
    public Vector3 colliderSize;
    public DefaultValue roarFrequency_countBySecond;
    public DefaultValue laserLength;
}