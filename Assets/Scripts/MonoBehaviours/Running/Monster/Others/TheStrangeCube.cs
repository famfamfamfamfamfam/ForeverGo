using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStrangeCube : MonoBehaviour
{
    [SerializeField]
    DefaultValue stayTime;

    private void OnEnable()
    {
        StartCoroutine(CountdownToDisapear((int)stayTime.value));
    }

    IEnumerator CountdownToDisapear(int time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
            MonstersManager.instance.ToIncreaseRangedMonstersHitTakableCount();
    }
}
