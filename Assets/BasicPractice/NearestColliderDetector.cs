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
    [SerializeField]
    int maxObjectPerCheck;
    Collider[] hits;
    LayerMask mask;
    Collider closestCollider;
    float currentSqrDistance, minSqrDistance;
    int currentHitsLength;

    void Start()
    {
        hits = new Collider[maxObjectPerCheck];
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
                currentHitsLength = Physics.OverlapSphereNonAlloc(transform.position, checkRangeRadius / i, hits, mask);
                if (currentHitsLength > 0)
                {
                    closestCollider = hits[0];
                    minSqrDistance = Vector3.SqrMagnitude(hits[0].transform.position - transform.position);
                    for (int index = 0; index < currentHitsLength; index++)
                    {
                        currentSqrDistance = Vector3.SqrMagnitude(hits[index].transform.position - transform.position);
                        if (currentSqrDistance < minSqrDistance)
                        {
                            minSqrDistance = currentSqrDistance;
                            closestCollider = hits[index];
                        }
                    }
                    Debug.Log(closestCollider.gameObject.name);
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
