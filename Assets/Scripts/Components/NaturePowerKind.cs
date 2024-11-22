using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kind", menuName = "Power Kind")]
public class NaturePowerKind : ScriptableObject
{
    public PlayerPowerKind powerKind;
    public PlayerPowerKind unselectedKind;
}
public enum PlayerPowerKind
{
    Wind,
    Water,
    Fire
}

public class PowerData
{
    Dictionary<PlayerPowerKind, PlayerData> kindsOfData;
    public PowerData(Animator animator, int[] stateHashes)
    {
        kindsOfData = new Dictionary<PlayerPowerKind, PlayerData>()
        {
            { PlayerPowerKind.Wind,
                new PlayerData(new WindAnimationContainer(animator, stateHashes[9]),
                    Resources.Load<Material>("PlayerMat/Wind")) },
            { PlayerPowerKind.Water,
                new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]),
                Resources.Load<Material>("PlayerMat/Water")) },
            { PlayerPowerKind.Fire,
                new PlayerData(new FireAnimationContainer(animator, stateHashes[11]),
                    Resources.Load<Material>("PlayerMat/Fire")) }
        };
    }

    public PlayerData GetKindOfData(PlayerPowerKind kindOfPower)
    {
        return kindsOfData[kindOfPower];
    }

    int kindIndex;
    public void UnloadAssetsOnDestroy()
    {
        for (; kindIndex < kindsOfData.Count; kindIndex++)
        {
            Resources.UnloadAsset(kindsOfData[(PlayerPowerKind)kindIndex].currentMaterial);
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
