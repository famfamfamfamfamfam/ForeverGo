using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData
{
    Dictionary<PowerKind, AnimationContainer> animContainerDictionary;
    public PlayerData(Animator animator, int[] stateHashes)
    {
        animContainerDictionary = new Dictionary<PowerKind, AnimationContainer>()
        {
            { PowerKind.Wind, new WindAnimationContainer(animator, stateHashes[9]) },
            { PowerKind.Water, new WaterAnimationContainer(animator, stateHashes[10]) },
            { PowerKind.Fire, new FireAnimationContainer(animator, stateHashes[11]) },
        };
    }

    public AnimationContainer GetYourAnimationContainer(PowerKind powerKind, PowerKind unselectedKind)
    {
        animContainerDictionary.Remove(unselectedKind);
        return animContainerDictionary[powerKind];
    }
}
