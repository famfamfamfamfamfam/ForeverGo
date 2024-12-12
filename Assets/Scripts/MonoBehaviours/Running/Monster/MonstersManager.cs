using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class MonstersManager : MonoBehaviour
{
    public static MonstersManager instance;

    [SerializeField]
    List<RuntimeAnimatorController> animatorControllers;
    [SerializeField]
    Transform[] wayPoints;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    GameObject strangeCubePrefab;

    GameObject strangeCubeInScene;

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
    private void OnDisable()
    {
        StopAllCoroutines();
        instance = null;
    }

    int monstersCountInLevel = 4;
    MonsterController monsterController;
    private void Start()
    {
        Vector3 standPosition;
        for (int i = 0; i < monstersCountInLevel; i++)
        {
            standPosition = wayPoints[i].position;
            Instantiate(prefab, standPosition, RotationLookingToCenterPoint(standPosition));
        }
        strangeCubeInScene = Instantiate(strangeCubePrefab);
        int index = 0;
        int subIndex = index;
        foreach (GameObject monster in monsters)
        {
            MonsterChip monsterChip = monster.GetComponent<MonsterChip>();
            monsterChip?.Init(monsterPowerKinds[subIndex], monsterFightTypes[index], animatorControllers[index]);
            CommonUtils.Instance.SetUpNextValue(ref index, monsterFightTypes.Length);
            if (index % 2 == 0)
                CommonUtils.Instance.SetUpNextValue(ref subIndex, monsterPowerKinds.Length);
            monsterController = monster.GetComponent<MonsterController>();
            if (monsterController is RangedMonsterController rangedMonster)
            {
                rangedMonsters.Add(rangedMonster);
            }
            else if (monsterController is MeleeMonsterController meleeMonster)
            {
                meleeMonsters.Add(meleeMonster);
            }
            sqrOverlapLength = overlapLength * overlapLength;
            StartCoroutine(CheckDistancesAndTearMeleeMonstersCollider());
        }
    }


    List<MeleeMonsterController> meleeMonsters = new List<MeleeMonsterController>();
    Vector3 currentCenter, directionToTear;
    float overlapLength = 1 / 2f;
    void ToTearOutMeleeMonstersCollider(bool hasOverlap)
    {
        if (hasOverlap)
        {
            foreach (MeleeMonsterController monster in meleeMonsters)
            {
                directionToTear = monster.transform.position - currentCenter;
                directionToTear.y = 0;
                directionToTear = overlapLength * directionToTear.normalized;
                monster.transform.position += directionToTear;
            }
        }
    }

    void CalculateTheCenterOfColliders()
    {
        Vector3 sum = Vector3.zero;
        foreach (MeleeMonsterController monster in meleeMonsters)
        {
            sum += monster.transform.position;
        }
        currentCenter = sum / meleeMonsters.Count;
    }

    float sqrOverlapLength;
    bool HasOverlap()
    {
        foreach (MeleeMonsterController monster in meleeMonsters)
        {
            if (Vector3.SqrMagnitude(monster.transform.position - currentCenter) <= sqrOverlapLength)
                return true;
        }
        return false;
    }

    IEnumerator CheckDistancesAndTearMeleeMonstersCollider()
    {
        while (!GameManager.instance.gameOver)
        {
            yield return new WaitForSeconds(1);
            CalculateTheCenterOfColliders();
            ToTearOutMeleeMonstersCollider(HasOverlap());
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

    List<RangedMonsterController> rangedMonsters = new List<RangedMonsterController>();
    int centerPointCount = 1;
    public void ToTurnTheRangedMonsters()
    {
        foreach (RangedMonsterController monster in rangedMonsters)
        {
            monster.gameObject.GetComponent<Animator>().applyRootMotion = false;
            int index = monster.transformSign;
            CommonUtils.Instance.SetUpNextValue(ref index, wayPoints.Length - centerPointCount);
            monster.transform.position = wayPoints[index].transform.position;
        }
    }

    public void SetUpTheCube(Vector3 beforeDisapearedMonsterPosition)
    {
        strangeCubeInScene.transform.position = new Vector3(
            beforeDisapearedMonsterPosition.x,
            strangeCubeInScene.transform.position.y,
            beforeDisapearedMonsterPosition.z);
        strangeCubeInScene.SetActive(true);
    }

    public void ToIncreaseRangedMonstersHitTakableCount()
    {
        foreach (RangedMonsterController monster in rangedMonsters)
        {
            monster.hitTakableCount++;
            Debug.Log(monster.hitTakableCount);
        }
    }
}
