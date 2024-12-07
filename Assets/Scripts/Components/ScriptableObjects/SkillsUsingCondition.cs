using UnityEngine;

[CreateAssetMenu(fileName = "New Using Condition", menuName = "Using Condition")]
public class SkillsUsingCondition : ScriptableObject
{
    public Skill skillKind;
    public int afterHitCount;
}

public enum Skill
{
    SuperSkill,
    UniqueSkill
}