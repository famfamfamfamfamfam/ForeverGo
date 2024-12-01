using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonstersManager : MonoBehaviour
{
    public static MonstersManager instance;
    public List<GameObject> monsters { get; private set; }

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
        instance = null;
        //release collections
    }

    private void Start()
    {
        foreach (GameObject monster in monsters)
        {
            MonsterChip monsterChip = monster.GetComponent<MonsterChip>();
            monsterChip?.SetPowerKind(PowerKind.Wind);

            monsterChip?.Init();
        }
    }
}
