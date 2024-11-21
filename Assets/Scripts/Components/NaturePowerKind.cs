using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Kind", menuName = "Power Kind")]
public class NaturePowerKind : ScriptableObject
{
    public PlayerPowerKind powerKind;
}
public enum PlayerPowerKind
{
    Wind,
    Water,
    Fire
}

public class PowerData
{
    Dictionary<PlayerPowerKind, AnimationContainer> kindsOfAnimationContainer;
    public PowerData(Animator animator, int[] stateHashes)
    {
        kindsOfAnimationContainer = new Dictionary<PlayerPowerKind, AnimationContainer>()
        {
            {PlayerPowerKind.Wind, new WindAnimationContainer(animator, stateHashes[9]) },
            {PlayerPowerKind.Water, new WaterAnimationContainer(animator, stateHashes[10]) },
            {PlayerPowerKind.Fire, new FireAnimationContainer(animator, stateHashes[11]) }
        };
    }

    public AnimationContainer GetKindOfAnimationContainer(PlayerPowerKind kindOfPower)
    {
        return kindsOfAnimationContainer[kindOfPower];
    }
}
