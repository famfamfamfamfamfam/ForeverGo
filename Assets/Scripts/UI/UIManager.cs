using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    GameObject configScreen, goToGameScreen, doneButton, quitButton;
    [SerializeField]
    Image monsterPortrait;

    private void Awake()
    {
        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        CommonUtils.Instance.onlyOneMode = false;
    }

    private void OnEnable()
    {
        instance = this;
    }
    [SerializeField]
    List<GameObject> objRunningOnDisable;
    void OnDestroy()
    {
        RefToAssets.refs._avtsDictionary.Clear();
        foreach (GameObject obj in objRunningOnDisable)
        {
            Destroy(obj);
        }
        instance = null;
    }
    private void Start()
    {
        CommonUtils.Instance.playerPower = new SelectedPowerKind();
        CommonUtils.Instance.monstersPower = new SelectedPowerKind();
        CommonUtils.Instance.monstersPower.selectedPowerKinds[0] = CommonUtils.Instance.RandomMonsterKind();
        CommonUtils.Instance.monstersPower.selectedPowerKinds[1] = CommonUtils.Instance.RandomMonsterKind();
        monsterPortrait.sprite = RefToAssets.refs._avtsDictionary[CommonUtils.Instance.monstersPower.selectedPowerKinds[0]];
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

    int index = 0;
    public void ToSetUpTheSelectedPower(PowerKind powerKind)
    {
        CommonUtils.Instance.playerPower.selectedPowerKinds[index] = powerKind;
        CommonUtils.Instance.SetUpNextValue(ref index, CommonUtils.Instance.playerPower.selectedPowerKinds.Length);
        Debug.Log(CommonUtils.Instance.playerPower.selectedPowerKinds[0] + "" + CommonUtils.Instance.playerPower.selectedPowerKinds[1]);
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
        if (hasAnotherSelected)
        {
            CommonUtils.Instance.onlyOneMode = !CommonUtils.Instance.onlyOneMode;
            doneButton.SetActive(!doneButton.activeSelf);
            ToDisplayQuitButton();
        }
    }
}
