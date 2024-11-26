using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProcess : MonoBehaviour
{
    NaturePowerKind selfPowerKind;
    PowerKind thisCharPowerKind;
    private void OnTriggerEnter(Collider other)
    {
        thisCharPowerKind = selfPowerKind.powerKind;
        //other.gameObject.GetComponent<NaturePowerKind>()?.powerKind
    }

    void ToPickTheDamage()
    {

    }
}
