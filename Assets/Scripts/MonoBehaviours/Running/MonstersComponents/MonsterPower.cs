using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPower : MonoBehaviour
{
    PowerKind powerKind;
    CharacterKind monsterChar = CharacterKind.Monster;

    public void Init(PowerKind kind, Renderer[] monsterRenderers)
    {
        powerKind = kind;
        foreach (Renderer renderer in monsterRenderers)
        {
            renderer.material = RefToAssets.refs._skinsDictionary[(kind, monsterChar)];
        }
    }
}
