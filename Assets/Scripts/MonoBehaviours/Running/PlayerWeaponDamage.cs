using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    NaturePowerKind playerPowerKindSO;
    PowerKind playerPowerKind;
    private void OnTriggerEnter(Collider other)
    {
        playerPowerKind = playerPowerKindSO.powerKind;
        //other.gameObject.GetComponent<NaturePowerKind>()?.powerKind
    }
}
