using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class MonstersManager : MonoBehaviour, ILoadingInLevel
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

    public GameObject boss { get; private set; }
    string bossPrefabPath = "Boss";
    public Dictionary<int, Action> initActionsInLevel => new Dictionary<int, Action>()
    {
        { 1, () => InitMonstersInFirstLevel() },
        { 2, () =>
            {
                GameObject bossPrefab = Resources.Load<GameObject>(bossPrefabPath);
                boss = Instantiate(bossPrefab, wayPoints[4].position, Quaternion.identity);
            }
        }
    };

    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        instance = null;
    }

    int monstersCountInLevel = 4;
    MonsterController monsterController;
    
    void InitMonstersInFirstLevel()
    {
        monsters = new List<GameObject>();
        monsterPowerKinds = PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Monster).selectedPowerKinds;
        monsterFightTypes = new Type[2]
        {
            typeof(MeleeMonsterController),
            typeof(RangedMonsterController)
        };

        Vector3 standPosition;
        for (int i = 0; i < monstersCountInLevel; i++)
        {
            standPosition = wayPoints[i].position;
            GameObject monster = Instantiate(prefab, standPosition, RotationLookingToCenterPoint(standPosition));
            monsters.Add(monster);
        }
        strangeCubeInScene = Instantiate(strangeCubePrefab);
        int index = 0;
        int subIndex = index;
        foreach (GameObject monster in monsters)
        {
            MonsterChip monsterChip = monster.GetComponent<MonsterChip>();
            monsterChip?.Init(monsterPowerKinds[subIndex], monsterFightTypes[index], animatorControllers[index]);
            CommonUtils.instance.SetUpNextIndex(ref index, monsterFightTypes.Length);
            if (index % 2 == 0)
                CommonUtils.instance.SetUpNextIndex(ref subIndex, monsterPowerKinds.Length);
            monsterController = monster.GetComponent<MonsterController>();
            if (monsterController is RangedMonsterController rangedMonster)
                rangedMonsters.Add(rangedMonster);
            else if (monsterController is MeleeMonsterController meleeMonster)
                meleeMonsters.Add(meleeMonster);
            sqrOverlapLength = overlapLength * overlapLength;
            StartCoroutine(CheckDistancesAndTearMeleeMonstersCollider());
        }
    }

    public void RemoveMonsterFromList(GameObject monster)
    {
        monsters.Remove(monster);
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        if (monsterController is RangedMonsterController rangedController)
            rangedMonsters.Remove(rangedController);
        if (monsterController is MeleeMonsterController meleeController)
            meleeMonsters.Remove(meleeController);
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

    int checkFrequency_countBySecond = 1;
    IEnumerator CheckDistancesAndTearMeleeMonstersCollider()
    {
        while (!GameManager.instance.gameOver && meleeMonsters.Count > 1)
        {
            yield return new WaitForSecondsRealtime(checkFrequency_countBySecond);
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
            CommonUtils.instance.SetUpNextIndex(ref index, wayPoints.Length - centerPointCount);
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
            GameManager.instance.Notify(TypeOfEvent.RangedMonstersHittableCountChange, monster.hitTakableCount);
            Debug.Log(monster.hitTakableCount);
        }
    }

    public void ToDecreaseRangedMonstersHitTakableCount()
    {
        foreach (RangedMonsterController monster in rangedMonsters)
        {
            monster.hitTakableCount--;
        }
    }


    [SerializeField]
    GameObject rockSpawner;
    public void TurnRockSpawnerOnBossStates()
    {
        rockSpawner.SetActive(!rockSpawner.activeSelf);
    }
}
