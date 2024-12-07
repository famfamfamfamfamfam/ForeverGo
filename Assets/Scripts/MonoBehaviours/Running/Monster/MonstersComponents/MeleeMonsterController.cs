using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TreeEditor;
using System.Linq;

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
    float checkDistance;

    private void Update()
    {
        if (Time.frameCount % changeRangeFrequency_countByFrame == 0)
        {
            adjustDistance = UnityEngine.Random.Range(min, max);
            checkDistance = (startHandsAttackingDistance + adjustDistance) * (startHandsAttackingDistance + adjustDistance);
        }
        if (Vector3.SqrMagnitude(transform.position - MonstersManager.instance._player.transform.position)
            <= checkDistance)
        {

        }
    }

    void NavigateMonster()
    {

    }

    int raycastDistance = 30;
    SortedDictionary<float, Transform> transformSortedDictionary = new SortedDictionary<float, Transform>();
    Vector3[] checkedDirections;
    void ToFleeOnLowHP()
    {
        checkedDirections = new Vector3[] { -transform.forward, transform.right, -transform.right };
        LayerMask layerMask = LayerMask.GetMask("Rails");
        foreach (Vector3 dir in checkedDirections)
        {
            FindTheFarestDistance(dir, layerMask);
        }
        Transform targetToFlee = transformSortedDictionary.Values.Last();
        transform.forward = targetToFlee.position - transform.position;
        //run to an distance
    }
    void FindTheFarestDistance(Vector3 checkedDirection, LayerMask layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, checkedDirection, out hit, raycastDistance, layerMask))
        {
            float checkedDistance = Vector3.SqrMagnitude(hit.transform.position - transform.position);
            transformSortedDictionary.Add(checkedDistance, hit.transform);
        }
    }
}
