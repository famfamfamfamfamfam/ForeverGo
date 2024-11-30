using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProcess : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.OnAttack(gameObject, other.gameObject);
    }
}
