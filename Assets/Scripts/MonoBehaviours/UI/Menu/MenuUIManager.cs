using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;
    [SerializeField]
    GameObject configScreen, goToGameScreen, doneButton, quitButton;
    [SerializeField]
    Image monsterPortrait;

    private void Awake()
    {
        if (LevelManager.instance != null)
        {
            Destroy(LevelManager.instance.gameObject);
            LevelManager.instance = null;
        }
        PlayerSelectionData.EmptyTheInstance(this);

        PlayerSelectionData.Instance.SetOnlyOneMode(this, false);
    }

    private void OnEnable()
    {
        instance = this;
    }
    [SerializeField]
    List<GameObject> objRunningOnDisable;
    void OnDestroy()
    {
        RefToAssets.refs.ReleaseAvatarsDictionary();
        foreach (GameObject obj in objRunningOnDisable)
        {
            Destroy(obj);
        }
        instance = null;
    }
    private void Start()
    {
        PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Monster).selectedPowerKinds[0] = CommonUtils.instance.RandomMonsterKind();
        PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Monster).selectedPowerKinds[1] = CommonUtils.instance.RandomMonsterKind();
        monsterPortrait.sprite = RefToAssets.refs._avtsDictionary[PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Monster).selectedPowerKinds[0]];
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
        PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Player).selectedPowerKinds[index] = powerKind;
        CommonUtils.instance.SetUpNextValue(ref index, PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Player).selectedPowerKinds.Length);
        Debug.Log(PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Monster).selectedPowerKinds[0] + "" + PlayerSelectionData.Instance.GetSelectedPower(this, CharacterKind.Player).selectedPowerKinds[1]);
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
            PlayerSelectionData.Instance.SetOnlyOneMode(this, !PlayerSelectionData.Instance.onlyOneMode);
            doneButton.SetActive(!doneButton.activeSelf);
            ToDisplayQuitButton();
        }
    }
}
