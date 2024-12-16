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

        eventsDictionary = new Dictionary<TypeOfEvent, Delegate>
        {
            { TypeOfEvent.PlayerHPChange, PlayerHPChange },
            { TypeOfEvent.MonstersHPChange, MonstersHPChange },
            { TypeOfEvent.PlayerMarkChange, PlayerMarkChange },
            { TypeOfEvent.PlayerSuperSkillStatusChange, PlayerSuperSkillStatusChange },
            { TypeOfEvent.PlayerUniqueSkillStatusChange, PlayerUniqueSkillStatusChange },
            { TypeOfEvent.RangedMonstersHittableCountChange, RangedMonstersHittableCountChange },
            { TypeOfEvent.HasPlayerDamageDealt, HasPlayerDamageDealt }
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


    public event Action<(Renderer, MaterialPropertyBlock, float)> MonstersHPChange;
    public event Action<float> PlayerHPChange;
    public event Action<string> PlayerMarkChange;
    public event Action<float> PlayerSuperSkillStatusChange;
    public event Action<int> PlayerUniqueSkillStatusChange;
    public event Action<int> RangedMonstersHittableCountChange;
    public event Action<(float, float, int)> HasPlayerDamageDealt;

    Dictionary<TypeOfEvent, Delegate> eventsDictionary;
    public void Notify<T>(TypeOfEvent eventType, T param)
    {
        if (eventsDictionary[eventType] is Action<T> action)
        {
            action.Invoke(param);
        }
    }

}

public enum TypeOfEvent
{
    MonstersHPChange,
    PlayerHPChange,
    PlayerMarkChange,
    PlayerSuperSkillStatusChange,
    PlayerUniqueSkillStatusChange,
    RangedMonstersHittableCountChange,
    HasPlayerDamageDealt
}