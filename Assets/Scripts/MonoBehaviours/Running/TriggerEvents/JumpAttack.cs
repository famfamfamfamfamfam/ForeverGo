using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.root.gameObject.GetComponent<MonsterController>()?.ToJumpAttack();
    }
}
