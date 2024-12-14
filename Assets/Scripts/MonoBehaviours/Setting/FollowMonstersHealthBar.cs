using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMonstersHealthBar : MonoBehaviour
{
    Vector3 direction;
    void Start()
    {
        LookAtCamera();
    }

    void Update()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
        direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
