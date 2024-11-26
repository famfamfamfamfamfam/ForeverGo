using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    GameObject configScreen, goToGameScreen, doneButton, quitButton;
    [SerializeField]
    NaturePowerKind playerPowerData, monstersFirstPowerData, monstersSecondPowerData;
    [SerializeField]
    Image monsterPortrait;

    private void Awake()
    {
        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        CommonMethods.Instance.onlyOneMode = false;
    }

    private void OnEnable()
    {
        instance = this;
    }
    [SerializeField]
    List<GameObject> objRunningOnDisable;
    void OnDestroy()
    {
        RefToAssets.refs.avtsDictionary.Clear();
        foreach (GameObject obj in objRunningOnDisable)
        {
            Destroy(obj);
        }
        instance = null;
    }
    private void Start()
    {
        monstersFirstPowerData.powerKind = CommonMethods.Instance.RandomMonsterKind(ref monstersFirstPowerData.unselectedKind);
        monstersSecondPowerData.powerKind = CommonMethods.Instance.RandomMonsterKind(ref monstersSecondPowerData.unselectedKind);
        monsterPortrait.sprite = RefToAssets.refs.avtsDictionary[monstersFirstPowerData.powerKind];
        goToGameScreen.SetActive(false);
        doneButton.SetActive(false);
    }

    public bool hasAnotherSelected { get; private set; }
    public void ToReceiveSelection(bool selected, ref bool status, GameObject obj)
    {
        if (selected)
        {
            if (doneButton.activeSelf)
                return;
            else if (hasAnotherSelected)
                doneButton.SetActive(true);
            else
                hasAnotherSelected = true;
            status = true;
            Debug.Log(obj + "selected");
        }
    }

    public void ToUnselect(bool selected, ref bool status, GameObject obj)
    {
        if (!selected)
        {
            status = false;
            if (doneButton.activeSelf)
            {
                doneButton.SetActive(false);
                Debug.Log(obj + "unselected");
                return;
            }
            hasAnotherSelected = false;
            Debug.Log(obj + "unselected");
        }
    }

    public bool DoneButtonIsActived()
    {
        return doneButton.activeSelf;
    }

    public void ToSetUpTheUnselectedPower(PowerKind powerKind)
    {
        playerPowerData.unselectedKind = powerKind;
    }

    public void ToSetUpTheSelectedPower(PowerKind powerKind)
    {
        playerPowerData.powerKind = powerKind;
    }

    public PowerKind ToGetKindOfPower(int id)
    {
        switch (id)
        {
            case 0: return PowerKind.Wind;
            case 1: return PowerKind.Water;
            default: return PowerKind.Fire;
        }
    }

    public void ToSwitchScreen()
    {
        configScreen.SetActive(!configScreen.activeSelf);
        goToGameScreen.SetActive(!goToGameScreen.activeSelf);
    }

    public void OnPressQuitButton()
    {
        Application.Quit();
    }

    public void ToDisplayQuitButton()
    {
        if (doneButton.activeSelf)
            quitButton.SetActive(false);
        else if (!hasAnotherSelected)
            quitButton.SetActive(true);
    }

    public void ToChooseOnlyOne()
    {
        CommonMethods.Instance.onlyOneMode = !CommonMethods.Instance.onlyOneMode;
        doneButton.SetActive(!doneButton.activeSelf);
        ToDisplayQuitButton();

    }
}
