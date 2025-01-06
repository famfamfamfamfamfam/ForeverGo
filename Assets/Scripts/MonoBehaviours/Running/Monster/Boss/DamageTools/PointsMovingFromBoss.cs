using UnityEngine;

public class PointsMovingFromBoss : MonoBehaviour
{
    [SerializeField]
    GameObject[] pointsMoving;
    [SerializeField]
    GameObject[] targetPoints;
    Vector3[] pointsStartPosition;
    Vector3[] targetPointsPosition;

    LineRenderer lineRenderer;
    bool isMovingOut, isMovingIn, hasStopped;
    float moveSpeed = 5f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        AttachLineToPoints();
        pointsStartPosition = new Vector3[pointsMoving.Length];
        targetPointsPosition = new Vector3[targetPoints.Length];
        for (int i = 0; i < pointsMoving.Length; i++)
        {
            pointsStartPosition[i] = pointsMoving[i].transform.position;
            targetPointsPosition[i] = targetPoints[i].transform.position;
            targetPointsPosition[i].y = pointsStartPosition[i].y;
        }
    }

    void AttachLineToPoints()
    {
        for (int i = 0; i < pointsMoving.Length; i++)
        {
            lineRenderer.SetPosition(i, pointsMoving[i].transform.position);
        }
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, pointsMoving[0].transform.position);
    }

    void Move(Vector3[] currentTarget, out bool isMoving)
    {
        for (int i = 0; i < pointsMoving.Length; i++)
        {
            pointsMoving[i].transform.position = Vector3.MoveTowards(
                pointsMoving[i].transform.position,
                currentTarget[i],
                moveSpeed * Time.deltaTime);
        }
        AttachLineToPoints();
        if (pointsMoving[0].transform.position == currentTarget[0])
            isMoving = false;
        else
            isMoving = true;
    }

    BossPower bossPower;
    public void AttackWithDynamicRange(BossPower bossComponent)
    {
        isMovingOut = true;
        isMovingIn = false;
        hasStopped = false;
        if (bossPower == null)
            bossPower = bossComponent;
        bossPower.SetTakeDamageState(true);
    }

    private void Update()
    {
        if (hasStopped)
            return;
        if (isMovingIn)
        {
            Move(pointsStartPosition, out bool isMoving);
            if (!isMoving)
            {
                hasStopped = true;
                bossPower?.SetTakeDamageState(false);
            }
        }
        if (isMovingOut)
        {
            Move(targetPointsPosition, out bool isMoving);
            if (!isMoving)
            {
                isMovingOut = false;
                isMovingIn = true;
            }
        }
    }

}
