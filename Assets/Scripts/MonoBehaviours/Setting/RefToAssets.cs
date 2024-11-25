using System.Collections;
using System.Collections.Generic;
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
        foreach (Skin skin in skins)
        {
            skinsDictionary.Add((skin.kind, skin.character), skin.material);
        }
        foreach (Avatar avatar in avatars)
        {
            avtsDictionary.Add(avatar.kind, avatar.portrait);
        }
    }
}
