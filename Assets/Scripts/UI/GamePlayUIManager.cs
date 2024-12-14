using System.Collections;
using System.Collections.Generic;
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

        playerCurrentMark.text = null;
        GameManager.instance.PlayerMarkChange += UpdatePlayerMark;

        playerDamageDealt.text = null;
    }
    private void OnDisable()
    {
        GameManager.instance.MonstersHPChange -= UpdateMonsterHealthBar;
        GameManager.instance.PlayerMarkChange -= UpdatePlayerMark;

        instance = null;
    }

    string materialPropertyName = "_offset";
    float monsterTotalHealth;
    void UpdateMonsterHealthBar(Material material, float data)
    {
        material.SetFloat(materialPropertyName, data / monsterTotalHealth);
    }

    void UpdatePlayerMark(string markName)
    {
        playerCurrentMark.text = markName;
    }
}
