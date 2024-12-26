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
    //Tắt capsule collider của player khi dash
    bool isMovingOut, isMovingIn;
    float moveSpeed = 15f;

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

    private void OnEnable()
    {
        isMovingOut = true;
        isMovingIn = false;
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
        isMoving = true;
    }

    private void Update()
    {
        if (isMovingIn)
        {
            Move(pointsStartPosition, out bool isMoving);
            if (!isMoving)
                return;
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
