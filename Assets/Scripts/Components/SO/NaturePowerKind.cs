using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Kind", menuName = "Power Kind")]
public class NaturePowerKind : ScriptableObject
{
    public PowerKind powerKind;
    public PowerKind unselectedKind;
}
public enum PowerKind
{
    Wind,
    Water,
    Fire
}