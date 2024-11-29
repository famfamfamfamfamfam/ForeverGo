using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Properties")]
public class CharacterProperties : ScriptableObject
{
    public CharacterKind character;
    public GeneralData properties;
}
