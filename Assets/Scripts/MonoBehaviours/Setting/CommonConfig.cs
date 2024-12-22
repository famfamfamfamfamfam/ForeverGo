using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonConfig : MonoBehaviour
{
    [SerializeField]
    List<Skin> skins;
    [SerializeField]
    List<Avatar> avatars;
    [SerializeField]
    List<Damage> damageConfig;

    public static CommonConfig instance;
    Dictionary<(PowerKind, CharacterKind), Material> skinsDictionary;
    public Dictionary<(PowerKind, CharacterKind), Material> _skinsDictionary { get => skinsDictionary; }

    Dictionary<PowerKind, Sprite> avtsDictionary;
    public Dictionary<PowerKind, Sprite> _avtsDictionary { get => avtsDictionary; }
    Dictionary<(PowerKind, CharacterKind), DamageConfig> damageDictionary;
    public Dictionary<(PowerKind, CharacterKind), DamageConfig> _damageDictionary { get => damageDictionary; }

    public Dictionary<PowerKind, AnimationContainer> playerAnimationContainer;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this);
        ConvertListToDictionary<Skin, (PowerKind, CharacterKind), Material>(skins, ref skinsDictionary,
            skin => (skin.kind, skin.character),
            skin => skin.material);
        ConvertListToDictionary<Avatar, PowerKind, Sprite>(avatars, ref avtsDictionary,
            avt => avt.kind,
            avt => avt.portrait);
        ConvertListToDictionary<Damage, (PowerKind, CharacterKind), DamageConfig>(damageConfig, ref damageDictionary,
            damage => (damage.powerKind, damage.character),
            damage => damage.data);
        foreach (DamageConfig damageData in damageDictionary.Values)
        {
            damageData.InitBonusDamageDictionary();
            damageData.InitPercentageDamageDictionary();
        }
    }

    void ConvertListToDictionary<ListType, KeysType, ValuesType>
        (List<ListType> elements, ref Dictionary<KeysType, ValuesType> dictionary,
        Func<ListType, KeysType> GetKeys, Func<ListType, ValuesType> GetValues)
    {
        dictionary = new Dictionary<KeysType, ValuesType>();
        foreach (ListType element in elements)
        {
            dictionary.Add(GetKeys(element), GetValues(element));
        }
        elements.Clear();
    }

    public void ReleaseAvatarsDictionary()
    {
        avtsDictionary = null;
    }

    public void InitAnimationContainerDictionary(Animator animator, int[] stateHashes)
    {
        playerAnimationContainer = new Dictionary<PowerKind, AnimationContainer>()
        {
            { PowerKind.Fire, new FireAnimationContainer(animator, stateHashes[11]) },
            { PowerKind.Water, new WaterAnimationContainer(animator, stateHashes[10]) },
            { PowerKind.Wind, new WindAnimationContainer(animator, stateHashes[9]) }
        };
    }
}