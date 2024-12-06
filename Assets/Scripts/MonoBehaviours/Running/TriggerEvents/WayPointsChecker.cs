using UnityEngine;

public class WayPointsChecker : MonoBehaviour
{
    [SerializeField]
    int indexInWayPointsList;

    RangedMonsterController rangedMonsterController;

    private void OnTriggerEnter(Collider other)
    {
        rangedMonsterController = other.gameObject.GetComponent<RangedMonsterController>();
        if (rangedMonsterController != null)
        {
            other.gameObject.GetComponent<Animator>().applyRootMotion = true;
            Transform monsterTransform = rangedMonsterController.transform;
            MonstersManager.instance.ToAttachToWayPoint(monsterTransform, indexInWayPointsList);
            rangedMonsterController.transformSign = indexInWayPointsList;
            monsterTransform.rotation = MonstersManager.instance.RotationLookingToCenterPoint(monsterTransform.position);
            StartCoroutine(rangedMonsterController.Roar());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rangedMonsterController = other.gameObject.GetComponent<RangedMonsterController>();
        if (rangedMonsterController != null)
            StopAllCoroutines();
    }
}
