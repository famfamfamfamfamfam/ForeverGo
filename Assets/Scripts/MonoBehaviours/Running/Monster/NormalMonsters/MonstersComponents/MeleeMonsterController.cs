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

    float checkHandsAttackingDistance;
    float checkFootAttackingDistance;

    float adjustDistance;
    float discoverPlayerDistance;
    float checkDiscoverPlayerDistance;
    float min, max;
    int changeRangeFrequency_countByFrame;


    private void Start()
    {
        checkFootAttackingDistance = Mathf.Pow(chip._meleeMonstersDefaultValues.startFootAttackingDistance.value, 2);
        checkHandsAttackingDistance = Mathf.Pow(chip._meleeMonstersDefaultValues.startHandsAttackingDistance.value, 2);

        playerLayerMask = LayerMask.GetMask("Player");
        railsLayerMask = LayerMask.GetMask("Rails");
        combineMask = playerLayerMask | railsLayerMask;
        checkRadius = chip._meleeMonstersDefaultValues.radiusOfTheRangeOfTrampleAttacking.value;
        discoverPlayerDistance = chip._meleeMonstersDefaultValues.discoverPlayerDistance.value;
        min = chip._meleeMonstersDefaultValues.minValueForAdjustingDiscoverPlayerDistance.value;
        max = chip._meleeMonstersDefaultValues.maxValueForAdjustingDiscoverPlayerDistance.value;
        changeRangeFrequency_countByFrame = (int)chip._meleeMonstersDefaultValues.changeRangeFrequency_countByFrame.value;
    }

    State currentState;
    float currentSqrDistance;
    private void Update()
    {
        if (GameManager.instance.gameOver || GameManager.instance.gamePause)
        {
            container.StopLoopAnimation(walkTransitionHash);
            if (!container.IsRunning(jumpAttackStateHash))
                container.TurnOnTemporaryAnimation(standTransitionHash, standStateHash);
            return;
        }

        if (Time.frameCount % changeRangeFrequency_countByFrame == 0)
        {
            adjustDistance = UnityEngine.Random.Range(min, max);
            checkDiscoverPlayerDistance = (discoverPlayerDistance + adjustDistance) * (discoverPlayerDistance + adjustDistance);
        }
        currentSqrDistance = Vector3.SqrMagnitude(transform.position - GameManager.instance._player.transform.position);
        if (!container.IsRunning(jumpAttackStateHash))
            Run(currentState);
        if (currentState == State.Flee)
            return;
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
    float maxAngle = 100f;
    public void NavigateMonster(Transform player)
    {
        targetAngle = Quaternion.LookRotation(player.position - transform.position);
        targetAngle = Quaternion.Euler(0, targetAngle.eulerAngles.y, 0);
        if (elapsedTime == 0)
            oldAngle = transform.rotation;
        if (Quaternion.Angle(oldAngle, targetAngle) > maxAngle)
            targetAngle = Quaternion.RotateTowards(oldAngle, targetAngle, maxAngle);
        elapsedTime = Mathf.Clamp01(elapsedTime + Time.deltaTime);
        transform.rotation = Quaternion.Slerp(oldAngle, targetAngle, elapsedTime);
        if (elapsedTime == 1)
            elapsedTime = 0;
    }

    int fleeTransitionHash_boolParam = Animator.StringToHash("isFleeing");

    int raycastDistance = 30;
    float[] distances = new float[4];
    Transform[] transformsBehindTheDistances = new Transform[4];
    Vector3[] checkedDirections;
    LayerMask railsLayerMask;
    public LayerMask combineMask { get; private set; }
    public void ToFleeOnLowHP()
    {
        currentState = State.Flee;
        container.StartLoopAnimation(fleeTransitionHash_boolParam);
        checkedDirections = new Vector3[] { transform.forward, -transform.forward, transform.right, -transform.right };
        for (int i = 0; i < checkedDirections.Length; i++)
        {
            FindTheFarestDistance(checkedDirections[i] + transform.up, i);
        }
        Array.Sort(distances, transformsBehindTheDistances);
        Transform targetToFlee = transformsBehindTheDistances[transformsBehindTheDistances.Length - 1];
        if (targetToFlee != null)
            SetNewForwardVector(targetToFlee.position);
    }
    float rayOriginHeight = 0.25f;
    void FindTheFarestDistance(Vector3 checkedDirection, int i)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOriginHeight * transform.up + transform.position, checkedDirection, out hit, raycastDistance, combineMask))
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
                NavigateMonster(GameManager.instance._player.transform);
                return;
            case State.Flee:
                if (chip._distancePoint.enabled)
                    chip._distancePoint.enabled = false;
                OnFleeState();
                return;
        }
    }

    int fleeTransitionHash_triggerParam = Animator.StringToHash("flee");
    int fleeStateHash = Animator.StringToHash("Base Layer.Fleeing");
    int standTransitionHash = Animator.StringToHash("stand");
    int standStateHash = Animator.StringToHash("Base Layer.Standing");
    void OnFleeState()
    {
        if (currentSqrDistance <= checkDiscoverPlayerDistance)
            container.TurnOnTemporaryAnimation(fleeTransitionHash_triggerParam, fleeStateHash);
        else
            container.TurnOnTemporaryAnimation(standTransitionHash, standStateHash);
    }

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            if (GameManager.instance.IsOutOfGround(transform.position) && currentState == State.Flee)
                ToFleeOnLowHP();
            transform.position += animator.deltaPosition;
        }
    }

    RaycastHit hit;
    Ray ray;
    int castSphereFrequency_countByFrame = 3;
    float sphereCastDistance = 1.25f;
    float sphereCastRadius = 0.75f;
    public void OnFleeAnimating()
    {
        if (Time.frameCount % castSphereFrequency_countByFrame == 0)
        {
            ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, sphereCastRadius, out hit, sphereCastDistance, combineMask))
                ToFleeOnLowHP();
        }
    }


    #region Animation Event
    float checkRadius;
    LayerMask playerLayerMask;
    public void TrampleAnimationEvent()
    {
        if (Physics.CheckSphere(chip._leftFoot.position, checkRadius, playerLayerMask))
            GameManager.instance.OnAttack(gameObject, GameManager.instance._player);
    }
    #endregion
}
