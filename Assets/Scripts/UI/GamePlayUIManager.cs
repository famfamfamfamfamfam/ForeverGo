using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIManager : MonoBehaviour
{
    public static GamePlayUIManager instance;

    [SerializeField]
    CharacterProperties playerOnlyMode, playerSwitchMode;
    [SerializeField]
    CharacterProperties monsters;

    [SerializeField]
    SkillsUsingCondition playerSuperSkill, playerUniqueSkill;

    [SerializeField]
    Slider playerHealthBar, playerSuperSkillBar, playerUniqueSkillBar;

    [SerializeField]
    TextMeshProUGUI playerCurrentMark, playerDamageDealt, playerStrangeCubeHitCount;

    private void OnEnable()
    {
        instance = this;
        monsterTotalHealth = monsters.properties._health;
        GameManager.instance.MonstersHPChange += UpdateMonsterHealthBar;

        if (CommonUtils.Instance.onlyOneMode)
            playerHealthBar.maxValue = playerOnlyMode.properties._health;
        else
            playerHealthBar.maxValue = playerSwitchMode.properties._health;
        playerHealthBar.value = playerHealthBar.maxValue;
        GameManager.instance.PlayerHPChange += UpdatePlayerHealthBar;

        playerCurrentMark.text = null;
        GameManager.instance.PlayerMarkChange += UpdatePlayerMark;

        playerSuperSkillBar.maxValue = playerSuperSkill.cooldown_second;
        GameManager.instance.PlayerSuperSkillStatusChange += UpdatePlayerSuperSkillBar;
        playerUniqueSkillBar.maxValue = playerUniqueSkill.afterHitCount;
        GameManager.instance.PlayerUniqueSkillStatusChange += UpdatePlayerUniqueSkillBar;

        GameManager.instance.RangedMonstersHittableCountChange += UpdateStrangeCubeHitCount;

        playerDamageDealt.text = null;
    }
    private void OnDisable()
    {
        GameManager.instance.MonstersHPChange -= UpdateMonsterHealthBar;
        GameManager.instance.PlayerHPChange -= UpdatePlayerHealthBar;
        GameManager.instance.PlayerSuperSkillStatusChange -= UpdatePlayerSuperSkillBar;
        GameManager.instance.PlayerUniqueSkillStatusChange -= UpdatePlayerUniqueSkillBar;
        GameManager.instance.PlayerMarkChange -= UpdatePlayerMark;
        GameManager.instance.RangedMonstersHittableCountChange -= UpdateStrangeCubeHitCount;

        instance = null;
    }

    string materialPropertyName = "_offset";
    float monsterTotalHealth;
    void UpdateMonsterHealthBar(Renderer renderer, MaterialPropertyBlock HPMatProperty, float data)
    {
        HPMatProperty.SetFloat(materialPropertyName, data / monsterTotalHealth);
        renderer.SetPropertyBlock(HPMatProperty);
    }

    void UpdatePlayerMark(string markName)
    {
        playerCurrentMark.text = markName;
    }

    void UpdatePlayerHealthBar(float data)
    {
        playerHealthBar.value = data;
    }

    void UpdatePlayerSuperSkillBar(float data)
    {
        playerSuperSkillBar.value = data;
    }

    void UpdatePlayerUniqueSkillBar(int data)
    {
        playerUniqueSkillBar.value = data;
    }

    void UpdateStrangeCubeHitCount(int data)
    {
        playerStrangeCubeHitCount.text = data.ToString();
    }
}
