using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData
{
    Dictionary<PowerKind, AnimationContainer> animContainerDictionary;
    public PlayerData(Animator animator, int[] stateHashes, PowerKind unselectedKind)
    {
        animContainerDictionary = new Dictionary<PowerKind, AnimationContainer>()
        {
            { PowerKind.Wind, new WindAnimationContainer(animator, stateHashes[9]) },
            { PowerKind.Water, new WaterAnimationContainer(animator, stateHashes[10]) },
            { PowerKind.Fire, new FireAnimationContainer(animator, stateHashes[11]) },
        };
        animContainerDictionary.Remove(unselectedKind);
        RefToAssets.refs.skinsDictionary.Remove((unselectedKind, CharacterKind.Player));
    }

    public AnimationContainer GetYourAnimationContainer(PowerKind powerKind)
    {
        return animContainerDictionary[powerKind];
    }
}
