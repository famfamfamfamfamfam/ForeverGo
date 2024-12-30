using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ILoadingInLevel
{
    public static GameManager instance;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject ground;
    [SerializeField]
    Transform[] rails;

    public GameObject _player { get => player; }

    private void Awake()
    {
        instance = this;
    }

    public bool gameOver { get; private set; }
    public bool gamePause { get; private set; }

    public Dictionary<int, Action> initActionsInLevel => new Dictionary<int, Action>()
    {
        { 1,  () => CommonInit() },
        { 2, () =>
            {
                CommonInit();
                mapRendered = MapPoints(out maxKeyOfMapPoints);
            }
        }
    };

    void CommonInit()
    {
        eventsDictionary = new Dictionary<TypeOfEvent, Delegate>();
        ArrangeRailsCoordinate();
    }

    public Dictionary<int, Vector3> mapRendered { get; private set; }
    int maxKeyOfMapPoints;
    public int maxKeyInMapRenderDictionary { get => maxKeyOfMapPoints; }

    public void SetGameOverState(GameOverState state)
    {
        Notify(TypeOfEvent.GameOver, state);
        UnlockTheCursor();
        gameOver = true;
    }

    public void PauseOrUnpauseGame()
    {
        gamePause = !gamePause;
        if (gamePause)
        {
            Time.timeScale = 0;
        }
        else
        {
            LockTheCursor();
            Time.timeScale = 1;
        }
        Notify(TypeOfEvent.GamePause, gamePause);
    }

    void LockTheCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockTheCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnAttack(GameObject attacker, GameObject damageTaker)
    {
        if (!gamePause && !gameOver)
        {
            IGetForAttacking iAttacker = attacker.GetComponent<IGetForAttacking>();
            if (iAttacker == null)
                return;
            PowerKind attackerPowerKind = iAttacker.GetDataForAttacking().powerKind;
            AttackState? attackerAttackState = iAttacker.GetDataForAttacking().attackState;
            damageTaker.GetComponent<IOnAttackable>()?.OnBeAttacked(attackerPowerKind, attackerAttackState);
            attacker.GetComponent<IHitCountForUsingSkillSettable>()?.SetHitCount();
        }
    }

    public void SetCurrentAttackState(AttackState? newAttackState, GameObject settableObj)
    {
        settableObj.GetComponent<IAttackStateSettable>()?.SetAttackState(newAttackState);
    }


    private void Start()
    {
        LockTheCursor();
        StartCoroutine(LoseDueToFalling());
    }

    float fallingDistance = 7.5f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && !gamePause)
            UnlockTheCursor();
        if (Input.GetKeyUp(KeyCode.LeftAlt) && !gamePause)
            LockTheCursor();
    }

    IEnumerator LoseDueToFalling()
    {
        yield return new WaitUntil(() => player.transform.position.y < ground.transform.position.y - fallingDistance);
        SetGameOverState(GameOverState.Lose);
        yield return new WaitForSecondsRealtime(1);
        player.GetComponent<Rigidbody>().isKinematic = true;
    }

    float[] railsXCoordinate = new float[4];
    float[] railsZCoordinate = new float[4];
    void ArrangeRailsCoordinate()
    {
        for (int i = 0; i < rails.Length; i++)
        {
            railsXCoordinate[i] = rails[i].position.x;
            railsZCoordinate[i] = rails[i].position.z;
        }
        Array.Sort(railsXCoordinate);
        Array.Sort(railsZCoordinate);
    }

    public bool IsOutOfGround(Vector3 currentPosition)
    {
        bool isOutOnX = currentPosition.x < railsXCoordinate[0] || currentPosition.x > railsXCoordinate[railsXCoordinate.Length - 1];
        bool isOutOnZ = currentPosition.z < railsZCoordinate[0] || currentPosition.z > railsZCoordinate[railsZCoordinate.Length - 1];
        return isOutOnX || isOutOnZ;
    }

    [SerializeField]
    Transform centerPoint;
    Dictionary<int, Vector3> MapPoints(out int maxKey)
    {
        maxKey = 0;
        int rowCount = (int)railsXCoordinate[railsXCoordinate.Length - 1];
        int colCount = (int)railsZCoordinate[railsZCoordinate.Length - 1];
        Dictionary<int, Vector3> result = new Dictionary<int, Vector3>();
        result.Add(maxKey, centerPoint.position);
        for (int i = 1 - colCount; i < colCount; i++)
        {
            for (int j = 1 - rowCount; j < rowCount; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                maxKey++;
                result.Add(maxKey, centerPoint.position + new Vector3(j, 0, i));
            }
        }
        return result;
    }


    Dictionary<TypeOfEvent, Delegate> eventsDictionary;

    public void Subscribe<T>(TypeOfEvent eventType, Action<T> subscribeMethod)
    {
        if (eventsDictionary.TryGetValue(eventType, out Delegate value))
            eventsDictionary[eventType] = Delegate.Combine(value, subscribeMethod);
        else
            eventsDictionary[eventType] = subscribeMethod;
    }

    public void Notify<T>(TypeOfEvent eventType, T param)
    {
        if (eventsDictionary[eventType] is Action<T> action)
            action.Invoke(param);
    }

    private void OnDisable()
    {
        eventsDictionary = null;
        Resources.UnloadUnusedAssets();
        instance = null;
    }
}

public enum GameOverState
{
    Lose,
    Win
}

public enum TypeOfEvent
{
    MonstersHPChange,
    PlayerHPChange,
    PlayerMarkChange,
    PlayerSuperSkillStateChange,
    PlayerUniqueSkillStateChange,
    RangedMonstersHittableCountChange,
    HasPlayerDamageDealt,
    GameOver,
    GamePause
}