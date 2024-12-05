using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReactionsADistanceAway : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.root.gameObject.GetComponent<MeleeMonsterController>()?.ToJumpAttack();
        Vector3 playerPosition = MonstersManager.instance._player.transform.position;
        gameObject.transform.root.gameObject.GetComponent<RangedMonsterController>()?.ToScream(playerPosition);
    }
}
