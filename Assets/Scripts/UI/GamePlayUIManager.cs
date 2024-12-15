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
        GameManager.instance.HasPlayerDamageDealt += UpdatePlayerDamageDealt;
    }
    private void OnDisable()
    {
        GameManager.instance.MonstersHPChange -= UpdateMonsterHealthBar;
        GameManager.instance.PlayerHPChange -= UpdatePlayerHealthBar;
        GameManager.instance.PlayerSuperSkillStatusChange -= UpdatePlayerSuperSkillBar;
        GameManager.instance.PlayerUniqueSkillStatusChange -= UpdatePlayerUniqueSkillBar;
        GameManager.instance.PlayerMarkChange -= UpdatePlayerMark;
        GameManager.instance.RangedMonstersHittableCountChange -= UpdateStrangeCubeHitCount;
        GameManager.instance.HasPlayerDamageDealt -= UpdatePlayerDamageDealt;

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
    bool beenCalled;
    void UpdatePlayerDamageDealt((float percentage, int bonusDamage) data)
    {
        playerDamageDealt.text = $"{data.percentage}% + {data.bonusDamage}";
        beenCalled = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(CountdownToDisapear());
    }

    int waitTime = 2;
    IEnumerator CountdownToDisapear()
    {
        yield return new WaitUntil(() => beenCalled == true);
        yield return new WaitForSeconds(waitTime);
        playerDamageDealt.text = null;
        beenCalled = false;
    }
}
