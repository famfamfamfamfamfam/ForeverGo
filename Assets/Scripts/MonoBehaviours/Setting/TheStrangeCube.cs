using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStrangeCube : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(CountdownToDisapear());
    }

    IEnumerator CountdownToDisapear()
    {
        yield return new WaitForSeconds(5);//can be config
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
            MonstersManager.instance.rangedMonstersHitTakableCount++;
        Debug.Log(MonstersManager.instance.rangedMonstersHitTakableCount);
    }
}
