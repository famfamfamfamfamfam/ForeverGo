using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class MeleeMonsterController : MonsterController
{
    Dictionary<int, Action> meeleeMonsterAttackDictionary;

    private new void Awake()
    {
        base.Awake();
        MonsterChip chip = GetComponent<MonsterChip>();
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

    float startHandsAttackingDistance = 6; //can be config
    float startFootAttackingDistance = 2;
    float min = 1f, max = 3f;
    int changeRangeFrequency_countByFrame = 150;

    float adjustDistance;
    float checkHandsAttackingDistance;

    private void Update()
    {
        if (Time.frameCount % changeRangeFrequency_countByFrame == 0)
        {
            adjustDistance = UnityEngine.Random.Range(min, max);
            checkHandsAttackingDistance = (startHandsAttackingDistance + adjustDistance) * (startHandsAttackingDistance + adjustDistance);
        }
        float currentSqrDistance = Vector3.SqrMagnitude(transform.position - MonstersManager.instance._player.transform.position);
        if (currentSqrDistance <= checkHandsAttackingDistance)
        {
            if (currentSqrDistance <= startFootAttackingDistance)
            {
                //foot attack
                return;
            }
            //hand attack
            return;
        }
        //run
    }

    public void NavigateMonster()// calls at onrunning
    {

    }

    int raycastDistance = 30;
    float[] distances = new float[3];
    Transform[] transformsFollowingDistances = new Transform[3];
    Vector3[] checkedDirections;
    public void ToFleeOnLowHP()
    {
        checkedDirections = new Vector3[] { -transform.forward, transform.right, -transform.right };
        LayerMask layerMask = LayerMask.GetMask("Rails");
        for (int i = 0; i < checkedDirections.Length; i++)
        {
            FindTheFarestDistance(checkedDirections[i], layerMask, i);
        }
        Array.Sort(distances, transformsFollowingDistances);
        Transform targetToFlee = transformsFollowingDistances[transformsFollowingDistances.Length - 1];
        transform.forward = targetToFlee.position - transform.position;
        //run to an distance, distance point collider adjust
    }
    void FindTheFarestDistance(Vector3 checkedDirection, LayerMask layerMask, int i)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, checkedDirection, out hit, raycastDistance, layerMask))
        {
            distances[i] = Vector3.SqrMagnitude(hit.transform.position - transform.position);
            transformsFollowingDistances[i] = hit.transform;
        }
    }
}
