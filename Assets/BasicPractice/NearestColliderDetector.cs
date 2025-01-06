using System.Collections;
using UnityEngine;

public class NearestColliderDetector : MonoBehaviour
{
    [SerializeField]
    string checkLayerName;
    [SerializeField]
    int checkFrequencyBySecond;
    [SerializeField]
    float checkRangeRadius;
    [SerializeField]
    int subdivisionCountPerCheck;
    RaycastHit[] hit;
    LayerMask mask;

    void Start()
    {
        hit = new RaycastHit[1];
        mask = LayerMask.GetMask(checkLayerName);
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkFrequencyBySecond);
            for (int i = subdivisionCountPerCheck; i > 0; i--)
            {
                if (Physics.SphereCastNonAlloc(transform.position, checkRangeRadius / i, transform.forward, hit, 0.01f, mask) > 0)
                {
                    Debug.Log(hit[0].collider.gameObject.name);
                    break;
                }
                yield return null;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
