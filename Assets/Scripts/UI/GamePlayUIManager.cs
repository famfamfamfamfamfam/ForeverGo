using System;
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
    TextMeshProUGUI playerCurrentMark, playerMainDamageDealt, playerBonusDamage, playerStrangeCubeHitCount;

    private void OnEnable()
    {
        instance = this;
        monsterTotalHealth = monsters.properties._health;
        GameManager.instance.Subscribe<(Renderer, MaterialPropertyBlock, float)>(TypeOfEvent.MonstersHPChange, data => UpdateMonsterHealthBar(data));

        if (CommonUtils.Instance.onlyOneMode)
            playerHealthBar.maxValue = playerOnlyMode.properties._health;
        else
            playerHealthBar.maxValue = playerSwitchMode.properties._health;
        playerHealthBar.value = playerHealthBar.maxValue;
        GameManager.instance.Subscribe<float>(TypeOfEvent.PlayerHPChange, data => UpdatePlayerHealthBar(data));

        playerCurrentMark.text = null;
        GameManager.instance.Subscribe<string>(TypeOfEvent.PlayerMarkChange, markName => UpdatePlayerMark(markName));

        playerSuperSkillBar.maxValue = playerSuperSkill.cooldown_second;
        GameManager.instance.Subscribe<float>(TypeOfEvent.PlayerSuperSkillStatusChange, data => UpdatePlayerSuperSkillBar(data));
        playerUniqueSkillBar.maxValue = playerUniqueSkill.afterHitCount;
        GameManager.instance.Subscribe<int>(TypeOfEvent.PlayerUniqueSkillStatusChange, data => UpdatePlayerUniqueSkillBar(data));

        GameManager.instance.Subscribe<int>(TypeOfEvent.RangedMonstersHittableCountChange, data => UpdateStrangeCubeHitCount(data));

        playerMainDamageDealt.text = null;
        playerBonusDamage.text = null;
        GameManager.instance.Subscribe<(float, float, int)>(TypeOfEvent.HasPlayerDamageDealt, data => UpdatePlayerDamageDealt(data));
    }
    private void OnDisable()
    {
        GameManager.instance.UnsubscirbeAll();
        instance = null;
    }

    string materialPropertyName = "_offset";
    float monsterTotalHealth;
    void UpdateMonsterHealthBar((Renderer _renderer, MaterialPropertyBlock HPMatProperty, float displayHP) data)
    {
        data.HPMatProperty.SetFloat(materialPropertyName, data.displayHP / monsterTotalHealth);
        data._renderer.SetPropertyBlock(data.HPMatProperty);
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

    Coroutine coroutine;
    void UpdatePlayerDamageDealt((float percentage, float monsterHealth, int bonusDamage) data)
    {
        float monsterHealth = Mathf.Max(data.monsterHealth, 0);
        playerMainDamageDealt.text = $"{data.percentage}% of {monsterHealth}";
        UpdatePlayerBonusDamageDealt(data.bonusDamage);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(CountdownToDisapear());
    }

    void UpdatePlayerBonusDamageDealt(int bonusDamage)
    {
        playerBonusDamage.text = $"and +{bonusDamage}";
    }

    int waitTime = 2;
    IEnumerator CountdownToDisapear()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        playerMainDamageDealt.text = null;
        playerBonusDamage.text = null;
    }
}
