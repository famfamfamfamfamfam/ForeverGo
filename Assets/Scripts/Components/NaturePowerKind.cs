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

public class PowerData //chua toi uu, lap code, lang phi bo nho
{
    Dictionary<PowerKind, CharacterData> kindsOfData;
    public PowerData(Animator animator, int[] stateHashes)
    {
        kindsOfData = new Dictionary<PowerKind, CharacterData>()
        {
            { PowerKind.Wind,
                new PlayerData(new WindAnimationContainer(animator,
                    stateHashes[9]),
                    Resources.Load<Material>("PlayerMat/Wind")) },
            { PowerKind.Water,
                new PlayerData(new WaterAnimationContainer(animator,
                    stateHashes[10]),
                    Resources.Load<Material>("PlayerMat/Water")) },
            { PowerKind.Fire,
                new PlayerData(new FireAnimationContainer(animator,
                    stateHashes[11]),
                    Resources.Load<Material>("PlayerMat/Fire")) }
        };
    }

    public PowerData()
    {
        kindsOfData = new Dictionary<PowerKind, CharacterData>()
        {
            { PowerKind.Wind,
                new MonstersData(Resources.Load<Sprite>("Wind"),
                    Resources.Load<Material>("MonsterMat/Wind")) },
            { PowerKind.Water,
                new MonstersData(Resources.Load<Sprite>("Water"),
                    Resources.Load<Material>("MonsterMat/Water")) },
            { PowerKind.Fire,
                new MonstersData(Resources.Load<Sprite>("Fire"),
                    Resources.Load<Material>("MonsterMat/Fire")) },
        };
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
    public MonstersData(Sprite monsterPortrait, Material monsterMat)
    {
        portrait = monsterPortrait;
        material = monsterMat;
    }
}

public class CharacterData
{
    public Material material { get; protected set; }
}
