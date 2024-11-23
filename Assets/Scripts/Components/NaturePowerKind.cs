using System.Collections;
using System.Collections.Generic;
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

public class PowerData
{
    Dictionary<PowerKind, PlayerData> kindsOfPlayerData;
    public PowerData(Animator animator, int[] stateHashes)
    {
        kindsOfPlayerData = new Dictionary<PowerKind, PlayerData>()
        {
            { PowerKind.Wind,
                new PlayerData(new WindAnimationContainer(animator, stateHashes[9]),
                    Resources.Load<Material>("PlayerMat/Wind")) },
            { PowerKind.Water,
                new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]),
                Resources.Load<Material>("PlayerMat/Water")) },
            { PowerKind.Fire,
                new PlayerData(new FireAnimationContainer(animator, stateHashes[11]),
                    Resources.Load<Material>("PlayerMat/Fire")) }
        };
    }

    Dictionary<PowerKind, MonstersData> kindsOfMonstersData;
    public PowerData()
    {
        kindsOfMonstersData = new Dictionary<PowerKind, MonstersData>()
        {
            { PowerKind.Wind, new MonstersData(Resources.Load<Sprite>("Wind")) },
            { PowerKind.Water, new MonstersData(Resources.Load<Sprite>("Water")) },
            { PowerKind.Fire, new MonstersData(Resources.Load<Sprite>("Fire")) },
        };
    }

    public PlayerData GetKindOfPlayerData(PowerKind kindOfPower)
    {
        return kindsOfPlayerData[kindOfPower];
    }

    int kindIndex;
    public void UnloadPlayerAssetsOnDestroy()
    {
        for (; kindIndex < kindsOfPlayerData.Count; kindIndex++)
        {
            Resources.UnloadAsset(kindsOfPlayerData[(PowerKind)kindIndex].currentMaterial);
        }
        kindIndex = 0;
    }

    public MonstersData GetKindOfMonsterData(PowerKind kindOfPower)
    {
        return kindsOfMonstersData[kindOfPower];
    }

    public void UnloadMonstersAssetsOnDestroy()
    {
        for (; kindIndex < kindsOfMonstersData.Count; kindIndex++)
        {
            Resources.UnloadAsset(kindsOfMonstersData[(PowerKind)kindIndex].portrait);
        }
        kindIndex = 0;
    }
}

public class PlayerData
{
    public AnimationContainer playerCurrentAnimContainer {  get; private set; }
    public Material currentMaterial {  get; private set; }
    public PlayerData(AnimationContainer animationContainer, Material material)
    {
        playerCurrentAnimContainer = animationContainer;
        currentMaterial = material;
    }
}

public class MonstersData
{
    public Sprite portrait {  get; private set; }
    public MonstersData(Sprite monsterPortrait)
    {
        portrait = monsterPortrait;
    }
}
