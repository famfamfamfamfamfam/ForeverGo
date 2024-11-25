using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Avatar", menuName = "Avatar")]
public class Avatar : ScriptableObject
{
    public PowerKind kind;
    public Sprite portrait;
}
