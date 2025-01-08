using UnityEngine;

public class FollowCam : MonoBehaviour, ILateUpdateMethodWaitingToRun
{
    [SerializeField]
    Transform controlHead, camHead;
    float xRotateSpeed = 240/*12*/;
    float yRotateSpeed = 80;
    float backDistance = 4f;
    float upwardDistance = 0.075f;

    Vector3 rotateVelocity = Vector3.zero;
    float smoothTime = 0.075f;
    float currentMouseXRotation;

    public void SetUpPosition()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                camHead.position
                - camHead.forward * backDistance
                + camHead.up * upwardDistance
                + camHead.rotation * Vector3.forward,
                ref rotateVelocity, smoothTime);
            currentMouseXRotation = Input.GetAxis("Mouse X") * xRotateSpeed * Time.deltaTime;
            controlHead.Rotate(0, currentMouseXRotation, 0);
            camHead.Rotate(-Input.GetAxis("Mouse Y") * yRotateSpeed * Time.deltaTime, currentMouseXRotation, 0);
            transform.rotation = Quaternion.LookRotation(camHead.position - transform.position);
            if (transform.position.y > 0.15f)
                return;
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
    }
  
    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }
    public void ResetSmoothTimeToDefault()
    {
        smoothTime = 0.075f;
    }

    public bool SmoothTimeIsZeroCurrently()
    {
        return smoothTime == 0;
    }
}
