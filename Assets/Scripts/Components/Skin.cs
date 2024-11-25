using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class Skin : ScriptableObject
{
    public CharacterKind character;
    public PowerKind kind;
    public Material material;
}

public enum CharacterKind
{
    Player,
    Monster
}