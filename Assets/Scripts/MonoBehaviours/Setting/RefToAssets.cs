using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class RefToAssets : MonoBehaviour
{
    [SerializeField]
    List<Skin> skins;
    [SerializeField]
    List<Avatar> avatars;

    public static RefToAssets refs;
    public Dictionary<(PowerKind, CharacterKind), Material> skinsDictionary { get; private set; }
    public Dictionary<PowerKind, Sprite> avtsDictionary { get; private set; }
    private void Awake()
    {
        if (refs != null)
            Destroy(refs.gameObject);
        refs = this;
        DontDestroyOnLoad(this);
        skinsDictionary = new Dictionary<(PowerKind, CharacterKind), Material>();
        avtsDictionary = new Dictionary<PowerKind, Sprite>();
        ConvertListToDictionary<Skin, (PowerKind, CharacterKind), Material>(skins, skinsDictionary,
            skin => (skin.kind, skin.character),
            skin => skin.material);
        ConvertListToDictionary<Avatar, PowerKind, Sprite>(avatars, avtsDictionary,
            avt => avt.kind,
            avt => avt.portrait);
    }

    void ConvertListToDictionary<ListType, KeysType, ValuesType>
        (List<ListType> elements, Dictionary<KeysType, ValuesType> dictionary,
        Func<ListType, KeysType> GetKeys, Func<ListType, ValuesType> GetValues)
    {
        foreach (ListType element in elements)
        {
            dictionary.Add(GetKeys(element), GetValues(element));
        }
        elements.Clear();
    }
}
