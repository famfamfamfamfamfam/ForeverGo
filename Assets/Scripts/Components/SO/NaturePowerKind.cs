using UnityEngine;

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