using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothing : StateMachineBehaviour
{
    [SerializeField]
    float smoothTime;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance._playerFollowCam.SetSmoothTime(smoothTime);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance._playerFollowCam.ResetSmoothTimeToDefault();
    }
}
