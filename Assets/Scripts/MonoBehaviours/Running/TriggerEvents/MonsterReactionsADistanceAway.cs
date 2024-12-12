using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReactionsADistanceAway : MonoBehaviour
{
    MonsterController monster;
    private void OnTriggerEnter(Collider other)
    {
        monster = gameObject.transform.root.gameObject.GetComponent<MonsterController>();
        if (monster != null)
        {
            if (monster is MeleeMonsterController meleeMonster)
                meleeMonster.ToJumpAttack();
            if (monster is RangedMonsterController rangedMonster)
            {
                Vector3 playerPosition = GameManager.instance._player.transform.position;
                rangedMonster.ToScream(playerPosition);
            }
        }
    }
}
