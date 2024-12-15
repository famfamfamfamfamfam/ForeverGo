using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject player;
    [SerializeField]
    Transform[] rails;

    public GameObject _player { get => player; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event Action OnGameOver;

    public bool gameOver { get; private set; }
    public bool gamePause { get; set; }

    public void SetGameOverState(bool state)
    {
        OnGameOver?.Invoke();
        gameOver = state;
    }

    public void OnAttack(GameObject attacker, GameObject damageTaker)
    {
        if (!gamePause && !gameOver)
        {
            IPowerKindGettable iAttacker = attacker.GetComponent<IPowerKindGettable>();
            if (iAttacker == null)
                return;
            PowerKind attackerPowerKind = iAttacker.GetPowerKind();
            IAttackStateGettable iAttack = attacker.GetComponent<IAttackStateGettable>();
            AttackState? attackerAttackState = iAttack?.GetAttackState();
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
        ArrangeRailsCoordinate();
        eventsDictionary = new Dictionary<TypeOfEvent, Action<object[]>>
        {
            { TypeOfEvent.PlayerHPChange, param => SetFloatParamAndCallUpEvent(param, PlayerHPChange) },
            { TypeOfEvent.MonstersHPChange, param => SetParamsAndCallUpMonstersHPChange(param) },
            { TypeOfEvent.PlayerMarkChange, param => SetParamAndCallUpPlayerMarkChange(param) },
            { TypeOfEvent.PlayerSuperSkillStatusChange, param => SetFloatParamAndCallUpEvent(param, PlayerSuperSkillStatusChange) },
            { TypeOfEvent.PlayerUniqueSkillStatusChange, param => SetIntParamAndCallUpEvent(param, PlayerUniqueSkillStatusChange) },
            { TypeOfEvent.RangedMonstersHittableCountChange, param => SetIntParamAndCallUpEvent(param, RangedMonstersHittableCountChange) }
        };
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


    public event Action<Renderer, MaterialPropertyBlock, float> MonstersHPChange;
    public event Action<float> PlayerHPChange;
    public event Action<string> PlayerMarkChange;
    public event Action<float> PlayerSuperSkillStatusChange;
    public event Action<int> PlayerUniqueSkillStatusChange;
    public event Action<int> RangedMonstersHittableCountChange;

    Dictionary<TypeOfEvent, Action<object[]>> eventsDictionary;
    public void Notify(TypeOfEvent eventType, params object[] parameters)
    {
        eventsDictionary[eventType](parameters);
    }

    void SetParamsAndCallUpMonstersHPChange(object[] parameters)
    {
        Renderer renderer = null;
        CheckAndSetUpParams<Renderer>(parameters, 0, ref renderer);
        MaterialPropertyBlock HPMatProperty = null;
        CheckAndSetUpParams<MaterialPropertyBlock>(parameters, 1, ref HPMatProperty);
        float monsterDisplayFloat = 0f;
        CheckAndSetUpParams<float>(parameters, 2, ref monsterDisplayFloat);
        MonstersHPChange?.Invoke(renderer, HPMatProperty, monsterDisplayFloat);
    }

    void SetParamAndCallUpPlayerMarkChange(object[] parameters)
    {
        string displayString = null;
        CheckAndSetUpParams<string>(parameters, 0, ref displayString);
        PlayerMarkChange?.Invoke(displayString);
    }

    void CheckAndSetUpParams<T>(object[] parameters, int checkIndex, ref T unitParam)
    {
        if (parameters.Length > 0 && parameters[checkIndex] is T)
            unitParam = (T)parameters[checkIndex];
    }

    void SetFloatParamAndCallUpEvent(object[] parameters, Action<float> ActionWithFloatParam)
    {
        float displayFloat = 0f;
        CheckAndSetUpParams<float>(parameters, 0, ref displayFloat);
        ActionWithFloatParam?.Invoke(displayFloat);
    }

    void SetIntParamAndCallUpEvent(object[] parameters, Action<int> ActionWithIntParam)
    {
        int displayInt = 0;
        CheckAndSetUpParams<int>(parameters, 0, ref displayInt);
        ActionWithIntParam?.Invoke(displayInt);
    }
}

public enum TypeOfEvent
{
    MonstersHPChange,
    PlayerHPChange,
    PlayerMarkChange,
    PlayerSuperSkillStatusChange,
    PlayerUniqueSkillStatusChange,
    RangedMonstersHittableCountChange
}