using UnityEngine;

public class WayPointsChecker : MonoBehaviour
{
    [SerializeField]
    int indexInWayPointsList;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<RangedMonsterController>()?.OnReachNewWayPoint(indexInWayPointsList);
    }
}
