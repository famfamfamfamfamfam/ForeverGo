using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonstersManager : MonoBehaviour
{
    public static MonstersManager instance;

    [SerializeField]
    List<RuntimeAnimatorController> animatorControllers;
    [SerializeField]
    Transform[] wayPoints;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject prefab;

    public List<GameObject> monsters { get; private set; }
    public GameObject _player { get => player; }

    Type[] monsterFightTypes;
    PowerKind[] monsterPowerKinds;

    private void Awake()
    {
        instance = this;
        monsters = new List<GameObject>();
        monsterPowerKinds = CommonUtils.Instance.monstersPower.selectedPowerKinds;
        monsterFightTypes = new Type[2]
        {
            typeof(MeleeMonsterController),
            typeof(RangedMonsterController)
        };
    }
    private void OnDisable()
    {
        instance = null;
    }

    int monstersCountInLevel = 4;
    private void Start()
    {
        Vector3 standPosition;
        for (int i = 0; i < monstersCountInLevel; i++)
        {
            standPosition = wayPoints[i].position;
            Instantiate(prefab, standPosition, RotationLookingToCenterPoint(standPosition));
        }

        int index = 0;
        int subIndex = index;
        foreach (GameObject monster in monsters)
        {
            MonsterChip monsterChip = monster.GetComponent<MonsterChip>();
            monsterChip?.Init(monsterPowerKinds[subIndex], monsterFightTypes[index], animatorControllers[index]);
            CommonUtils.Instance.SetUpNextValue(ref index, monsterFightTypes.Length);
            if (index % 2 == 0)
                CommonUtils.Instance.SetUpNextValue(ref subIndex, monsterPowerKinds.Length);
        }
    }

    public Quaternion RotationLookingToCenterPoint(Vector3 currentPosition)
    {
        Vector3 centerPoint = wayPoints[4].position;
        return Quaternion.LookRotation(centerPoint - currentPosition);
    }

    public void ToAttachToWayPoint(Transform attachedChar, int wayPointIndex)
    {
        attachedChar.position = wayPoints[wayPointIndex].position;
    }

    int centerPointCount = 1;
    public void ToTurnTheRangedMonsters()
    {
        foreach (GameObject monster in monsters)
        {
            RangedMonsterController rangedMonster = monster.GetComponent<RangedMonsterController>();
            if (rangedMonster != null)
            {
                int index = rangedMonster.transformSign;
                CommonUtils.Instance.SetUpNextValue(ref index, wayPoints.Length - centerPointCount);
                rangedMonster.transform.forward = wayPoints[index].position - rangedMonster.transform.position;
                rangedMonster.ToRun();
            }
        }
    }
}
