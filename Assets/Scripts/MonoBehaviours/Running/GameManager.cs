using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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


    public event Action<Material, float> MonstersHPChange;
    public event Action<float> PlayerHPChange;
    public event Action<string> PlayerMarkChange;

    public void Notify(TypeOfEvent eventType, float displayFloat = 0f, Material HPMat = null, string displayString = null)
    {
        switch (eventType)
        {
            case TypeOfEvent.PlayerHPChange:
                PlayerHPChange?.Invoke(displayFloat);
                return;
            case TypeOfEvent.MonstersHPChange:
                MonstersHPChange?.Invoke(HPMat, displayFloat);
                return;
            case TypeOfEvent.PlayerMarkChange:
                PlayerMarkChange?.Invoke(displayString);
                return;
        }
    }
}

public enum TypeOfEvent
{
    MonstersHPChange,
    PlayerHPChange,
    PlayerMarkChange
}