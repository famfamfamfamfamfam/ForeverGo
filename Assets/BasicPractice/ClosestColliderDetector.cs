using System.Collections;
using UnityEngine;

public class ClosestColliderDetector : MonoBehaviour
{
    [SerializeField]
    string checkLayerName;
    [SerializeField]
    int checkFrequencyBySecond;
    [SerializeField]
    float checkRangeRadius;
    [SerializeField]
    [Range(1,10)]
    int subdivisionCountPerCheck;
    [SerializeField]
    int maxObjectPerCheck;
    Collider[] hits;
    LayerMask mask;
    Collider closestCollider;
    float unitRadiusForCheck;
    float currentSqrDistance, minSqrDistance;
    int currentHitsLength;

    void Start()
    {
        unitRadiusForCheck = checkRangeRadius / subdivisionCountPerCheck;
        hits = new Collider[maxObjectPerCheck];
        mask = LayerMask.GetMask(checkLayerName);
        StartCoroutine(CheckLoop());
    }

    IEnumerator CheckLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkFrequencyBySecond);
            StartCoroutine(CheckProcess());
        }
    }

    IEnumerator CheckProcess(bool suddenCheck = false)
    {
        for (float i = unitRadiusForCheck; i <= checkRangeRadius; i += unitRadiusForCheck)
        {
            currentHitsLength = Physics.OverlapSphereNonAlloc(transform.position, i, hits, mask);
            if (currentHitsLength > 0)
            {
                FindClosestCollider();
                Debug.Log(closestCollider.gameObject.name);
                break;
            }
            yield return null;
        }
        if (suddenCheck)
        {
            StartCoroutine(CheckLoop());
        }
    }

    void FindClosestCollider()
    {
        closestCollider = hits[0];
        minSqrDistance = Vector3.SqrMagnitude(hits[0].transform.position - transform.position);
        for (int i = 0; i < currentHitsLength; i++)
        {
            currentSqrDistance = Vector3.SqrMagnitude(hits[i].transform.position - transform.position);
            if (currentSqrDistance < minSqrDistance)
            {
                minSqrDistance = currentSqrDistance;
                closestCollider = hits[i];
            }
        }
    }

    public void SuddenCheck()
    {
        StopAllCoroutines();
        StartCoroutine(CheckProcess(true));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            SuddenCheck();
    }
}
