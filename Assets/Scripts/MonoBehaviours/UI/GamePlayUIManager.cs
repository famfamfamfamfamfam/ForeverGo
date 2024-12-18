using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIManager : MonoBehaviour
{
    public static GamePlayUIManager instance;

    [SerializeField]
    GameObject pauseScreen, endScreen;

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

    [SerializeField]
    TextMeshProUGUI notifsForPlayer;

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

        GameManager.instance.Subscribe<GameOverState>(TypeOfEvent.GameOver, state => DisplayResult(state));
        GameManager.instance.Subscribe<bool>(TypeOfEvent.GamePause, pauseState => DisplayPauseGameScreen(pauseState));
        notifsForPlayer.text = null;
        GameManager.instance.Subscribe<(NotiType, string)>(TypeOfEvent.HasNotiForPlayer, data => UpdateNotiForPlayer(data));
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

    Coroutine damageDealtCoroutine;
    void UpdatePlayerDamageDealt((float percentage, float monsterHealth, int bonusDamage) data)
    {
        float monsterHealth = Mathf.Max(data.monsterHealth, 0);
        playerMainDamageDealt.text = $"{data.percentage}% of {monsterHealth}";
        UpdatePlayerBonusDamageDealt(data.bonusDamage);
        if (damageDealtCoroutine != null)
            StopCoroutine(damageDealtCoroutine);
        damageDealtCoroutine = StartCoroutine(DamageDealtCountdownToDisapear());
    }

    void UpdatePlayerBonusDamageDealt(int bonusDamage)
    {
        playerBonusDamage.text = $"and +{bonusDamage}";
    }

    int waitTime = 2;
    IEnumerator DamageDealtCountdownToDisapear()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        playerMainDamageDealt.text = null;
        playerBonusDamage.text = null;
    }

    Coroutine notiCoroutine;
    string temp;
    void UpdateNotiForPlayer((NotiType notiType, string noti) data)//CHƯA TEST
    {
        notifsForPlayer.text = data.noti;
        if (data.notiType == NotiType.Command)
            temp = data.noti;
        else if (data.notiType == NotiType.ReleaseCommand)
            temp = null;
        if (notiCoroutine != null)
            StopCoroutine(notiCoroutine);
        notiCoroutine = StartCoroutine(NotiCountDownToDisapear(data.notiType, temp));
    }

    IEnumerator NotiCountDownToDisapear(NotiType notiType, string previousNoti)
    {
        if (notiType == NotiType.Command)
            yield break;
        yield return new WaitForSecondsRealtime(waitTime);
        notifsForPlayer.text = temp;
    }

    string loseNoti = "YOU LOSE!";
    string winNoti = "YOU WIN!";
    [SerializeField]
    TextMeshProUGUI resultText;
    void DisplayResult(GameOverState state)
    {
        endScreen.SetActive(true);
        switch (state)
        {
            case GameOverState.Lose:
                resultText.text = loseNoti;
                return;
            case GameOverState.Win:
                resultText.text = winNoti;
                return;
        }
    }

    void DisplayPauseGameScreen(bool pauseState)
    {
        pauseScreen.SetActive(pauseState);
    }
}

public enum NotiType
{
    Command,
    ReleaseCommand,
    Announce
}
