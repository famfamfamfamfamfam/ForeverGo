using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeMonsterController : MonsterController
{
    Dictionary<int, Action> meeleeMonsterAttackDictionary;
    MonsterChip chip;
    private new void Awake()
    {
        base.Awake();
        chip = GetComponent<MonsterChip>();
        meeleeMonsterAttackDictionary = new Dictionary<int, Action>()
        {
            { Animator.StringToHash("Base Layer.SlapDownAttacking"),
                () => chip._leftHand.enabled = !chip._leftHand.enabled },
            { Animator.StringToHash("Base Layer.SwipeAttacking"),
                () => chip._rightHand.enabled = !chip._rightHand.enabled },
            { Animator.StringToHash("Base Layer.JumpAttacking"), () =>
                {
                    chip._leftHand.enabled = !chip._leftHand.enabled;
                    chip._rightHand.enabled = !chip._rightHand.enabled;
                }
            }
        };
    }

    public void TurnDamagingTool(int stateHash)
    {
        meeleeMonsterAttackDictionary[stateHash].Invoke();
    }

    int jumpAttackTransitionHash = Animator.StringToHash("jumpAttack");
    int jumpAttackStateHash = Animator.StringToHash("Base Layer.JumpAttacking");
    public void ToJumpAttack()
    {
        container.TurnOnTemporaryAnimation(jumpAttackTransitionHash, jumpAttackStateHash);
    }

    float startHandsAttackingDistance = 2.5f; //can be config
    float startFootAttackingDistance = 1;
    float checkHandsAttackingDistance, checkFootAttackingDistance;

    float adjustDistance;
    float discoverPlayerDistance = 4;
    float checkDiscoverPlayerDistance;
    float min = -1f, max = 2f;
    int changeRangeFrequency_countByFrame = 150;


    private void Start()
    {
        checkFootAttackingDistance = Mathf.Pow(startFootAttackingDistance, 2);
        checkHandsAttackingDistance = Mathf.Pow(startHandsAttackingDistance, 2);

        playerLayerMask = LayerMask.GetMask("Player");
        railsLayerMask = LayerMask.GetMask("Rails");
        combineMask = playerLayerMask | railsLayerMask;
    }

    State currentState;
    float currentSqrDistance;
    private void Update()
    {
        if (Time.frameCount % changeRangeFrequency_countByFrame == 0)
        {
            adjustDistance = UnityEngine.Random.Range(min, max);
            checkDiscoverPlayerDistance = (discoverPlayerDistance + adjustDistance) * (discoverPlayerDistance + adjustDistance);
        }
        currentSqrDistance = Vector3.SqrMagnitude(transform.position - MonstersManager.instance._player.transform.position);
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != jumpAttackStateHash)
            Run(currentState);
        if (currentState == State.Flee)
        {
            OnFleeState();
            return;
        }
        if (currentSqrDistance <= checkHandsAttackingDistance)
        {
            if (currentSqrDistance <= checkFootAttackingDistance)
                currentState = State.FootAttack;
            else
                currentState = State.HandsAttack;
        }
        else
        {
            currentState = State.Walk;
        }
    }

    Quaternion oldAngle, targetAngle;
    float elapsedTime;
    public void NavigateMonster(Transform player) // flee exit trigger
    {
        targetAngle = Quaternion.LookRotation(player.position - transform.position);
        targetAngle = Quaternion.Euler(0, targetAngle.eulerAngles.y, 0);
        if (elapsedTime == 0)
            oldAngle = transform.rotation;
        elapsedTime = Mathf.Clamp01(elapsedTime + Time.deltaTime);
        transform.rotation = Quaternion.Slerp(oldAngle, targetAngle, elapsedTime);
        if (elapsedTime == 1)
            elapsedTime = 0;
    }

    int raycastDistance = 30;
    float[] distances = new float[3];
    Transform[] transformsBehindTheDistances = new Transform[3];
    Vector3[] checkedDirections;
    LayerMask railsLayerMask;
    LayerMask combineMask;
    public void ToFleeOnLowHP()
    {
        currentState = State.Flee;
        checkedDirections = new Vector3[] { -transform.forward, transform.right, -transform.right };
        for (int i = 0; i < checkedDirections.Length; i++)
        {
            FindTheFarestDistance(checkedDirections[i] + transform.up, combineMask, i);
        }
        Array.Sort(distances, transformsBehindTheDistances);
        Transform targetToFlee = transformsBehindTheDistances[transformsBehindTheDistances.Length - 1];
        SetNewForwardVector(targetToFlee.position);
    }
    void FindTheFarestDistance(Vector3 checkedDirection, LayerMask layerMask, int i)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, checkedDirection, out hit, raycastDistance, layerMask))
        {
            distances[i] = Vector3.SqrMagnitude(hit.transform.position - transform.position);
            transformsBehindTheDistances[i] = hit.transform;
        }
    }

    public void SetNewForwardVector(Vector3 target)
    {
        Vector3 newDirection = target - transform.position;
        newDirection.y = 0;
        transform.forward = newDirection;
    }

    enum State
    {
        None,
        FootAttack,
        HandsAttack,
        Walk,
        Flee
    }

    int trampleTransitionHash = Animator.StringToHash("trample");
    int trampleStateHash = Animator.StringToHash("Base Layer.TrampleAttacking");
    int handsAttackTransitionHash = Animator.StringToHash("handsAttack");
    int startHandsAttackStateHash = Animator.StringToHash("Base Layer.SlapAttacking");
    int walkTransitionHash = Animator.StringToHash("isWalking");
    void Run(State state)
    {
        switch(state)
        {
            case State.None:
                return;
            case State.FootAttack:
                container.TurnOnTemporaryAnimation(trampleTransitionHash, trampleStateHash);
                return;
            case State.HandsAttack:
                container.StopLoopAnimation(walkTransitionHash);
                container.TurnOnTemporaryAnimation(handsAttackTransitionHash, startHandsAttackStateHash);
                return;
            case State.Walk:
                container.StartLoopAnimation(walkTransitionHash);
                NavigateMonster(MonstersManager.instance._player.transform);
                return;
            case State.Flee:
                if (chip._distancePoint.enabled)
                    chip._distancePoint.enabled = false;
                return;
        }
    }

    int fleeTrasitionHash = Animator.StringToHash("flee");
    int fleeStateHash = Animator.StringToHash("Base Layer.Fleeing");
    void OnFleeState()
    {
        if (currentSqrDistance <= checkDiscoverPlayerDistance)
        {
            container.TurnOnTemporaryAnimation(fleeTrasitionHash, fleeStateHash);
        }
    }

    #region Animation Event
    float checkRadius = 1.75f;
    LayerMask playerLayerMask;
    public void TrampleAnimationEvent()
    {
        if (Physics.CheckSphere(chip._leftFoot.position, checkRadius, playerLayerMask))
        {
            GameManager.instance.OnAttack(gameObject, MonstersManager.instance._player);
        }
    }
    #endregion
}
