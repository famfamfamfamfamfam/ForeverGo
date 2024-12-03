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
    private void OnDestroy()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            Destroy(monsters[i]);
        }
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
        foreach (GameObject monster in monsters)
        {
            MonsterChip monsterChip = monster.GetComponent<MonsterChip>();
            monsterChip?.Init(monsterPowerKinds[index], monsterFightTypes[index], animatorControllers[index]);
            CommonUtils.Instance.SetUpNextValue(ref index, monsterFightTypes.Length);
        }
    }

    public Quaternion RotationLookingToCenterPoint(Vector3 currentPosition)
    {
        Vector3 centerPoint = wayPoints[4].position;
        return Quaternion.LookRotation(centerPoint - currentPosition);
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
