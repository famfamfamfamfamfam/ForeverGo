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

public class PowerData
{
    Dictionary<PowerKind, CharacterData> kindsOfData;
    public PowerData(Animator animator, int[] stateHashes, PowerKind unselectedKind)
    {
        CharacterData windData = new PlayerData(new WindAnimationContainer(animator, stateHashes[9]),
                                                    Resources.Load<Material>("PlayerMat/Wind"));
        CharacterData waterData = new PlayerData(new WaterAnimationContainer(animator, stateHashes[10]),
                                                    Resources.Load<Material>("PlayerMat/Water"));
        CharacterData fireData = new PlayerData(new FireAnimationContainer(animator, stateHashes[11]),
                                                    Resources.Load<Material>("PlayerMat/Fire"));
        InitData(windData, waterData, fireData);
        ToRemoveUselessElement(unselectedKind);
    }

    public PowerData(PowerKind selectedKind, string[] linkToAssets, Type T)
    {
        CharacterData windData = new MonstersData(Resources.Load(linkToAssets[0], T));
        CharacterData waterData = new MonstersData(Resources.Load(linkToAssets[1], T));
        CharacterData fireData = new MonstersData(Resources.Load(linkToAssets[2], T));
        InitData(windData, waterData, fireData);
        ToRemoveUselessElement(selectedKind);
    }

    void InitData(CharacterData windData, CharacterData waterData, CharacterData fireData)
    {
        kindsOfData = new Dictionary<PowerKind, CharacterData>()
        {
            { PowerKind.Wind, windData },
            { PowerKind.Water, waterData },
            { PowerKind.Fire, fireData },
        };
    }

    void ToRemoveUselessElement(PowerKind theKeyKind)
    {
        if (GetKindOfData(theKeyKind) is PlayerData)
            kindsOfData.Remove(theKeyKind);
        else
        {
            PowerKind[] keys = kindsOfData.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == theKeyKind)
                    continue;
                kindsOfData.Remove(keys[i]);
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public CharacterData GetKindOfData(PowerKind kindOfPower)
    {
        return kindsOfData[kindOfPower];
    }

    public void UnloadAssetsOnDestroy()
    {
        foreach (CharacterData data in kindsOfData.Values)
        {
            Resources.UnloadAsset(data.material);
            if (data is MonstersData mon)
                Resources.UnloadAsset(mon.portrait);
        }
    }
}

public class PlayerData : CharacterData
{
    public AnimationContainer playerCurrentAnimContainer {  get; private set; }
    public PlayerData(AnimationContainer animationContainer, Material playerMaterial)
    {
        playerCurrentAnimContainer = animationContainer;
        material = playerMaterial;
    }
}

public class MonstersData : CharacterData
{
    public Sprite portrait {  get; private set; }
    public MonstersData(UnityEngine.Object monsterAsset)
    {
        if (monsterAsset is Sprite)
            portrait = (Sprite)monsterAsset;
        if (monsterAsset is Material)
            material = (Material)monsterAsset;
    }
}

public class CharacterData
{
    public Material material { get; protected set; }
}
