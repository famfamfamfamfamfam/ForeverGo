using System;
using UnityEngine;

public class MonsterChip : MonoBehaviour
{
    [SerializeField]
    GameObject body;
    [SerializeField]
    Transform laserStartPoint;
    [SerializeField]
    CharacterProperties monsterProperties;

    public Transform _laserStartPoint { get => laserStartPoint; }
    public CharacterProperties _monsterProperties { get => monsterProperties; }

    void OnEnable()
    {
        MonstersManager.instance.monsters.Add(gameObject);
    }

    public void Init(PowerKind powerKind, Type monsterFightType, RuntimeAnimatorController monsterAnimatorController)
    {
        Renderer[] renderers = body.GetComponentsInChildren<Renderer>();
        MonsterPower powerProcessor = gameObject.AddComponent<MonsterPower>();
        powerProcessor.Init(powerKind, renderers);
        MonsterController controller = (MonsterController)gameObject.AddComponent(monsterFightType);
        controller.Init(monsterAnimatorController);
    }

    private void OnDisable()
    {
        //MonstersManager.instance.monsters.Remove(gameObject);
    }
}
