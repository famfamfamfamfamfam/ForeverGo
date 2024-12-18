using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour, ILateUpdateMethodWaitingToRun
{
    [SerializeField]
    Transform followedObj;
    float rotateSpeed = 360;
    float backDistance = 4f;
    float upwardDistance = 0.5f;

    public void SetUpPosition()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
        {
            transform.position = followedObj.position
                - followedObj.forward * backDistance
                + followedObj.up * upwardDistance
                + followedObj.rotation * Vector3.forward;
            followedObj.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(followedObj.position - transform.position);
        }
    }
}
