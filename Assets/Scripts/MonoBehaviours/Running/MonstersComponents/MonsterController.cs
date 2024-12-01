using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    protected Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
