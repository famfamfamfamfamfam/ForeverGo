using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    GameObject configScreen, goToGameScreen, doneButton;
    [SerializeField]
    NaturePowerKind powerData;
    private void OnEnable()
    {
        instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }
    private void Start()
    {
        goToGameScreen.SetActive(false);
        doneButton.SetActive(false);
    }

    bool hasAnotherSelected;
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

    public void ToSetUpTheUnselectedPower(PlayerPowerKind powerKind)
    {
        powerData.unselectedKind = powerKind;
    }

    public void ToSetUpTheSelectedPower(PlayerPowerKind powerKind)
    {
        powerData.powerKind = powerKind;
    }

    public PlayerPowerKind ToGetKindOfPower(int id)
    {
        switch (id)
        {
            case 0: return PlayerPowerKind.Wind;
            case 1: return PlayerPowerKind.Water;
            default: return PlayerPowerKind.Fire;
        }
    }

    public void OnPressDoneButton()
    {
        configScreen.SetActive(false);
        goToGameScreen.SetActive(true);
    }
}
