using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject configScreen, goToGameScreen, doneButton;
    public static UIManager instance;
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
        powerData = GetComponent<NaturePowerKind>();
    }

    bool hasAnotherSelected = false;
    public void ToReceiveSelection(bool selected, PlayerPowerKind powerKind)
    {
        if (selected)
        {
            if (doneButton.activeSelf)
                return;
            else if (hasAnotherSelected)
                doneButton.SetActive(true);
            else
                hasAnotherSelected = true;
            powerData.powerKind = powerKind;
        }
    }

    public void ToUnselect(bool selected)
    {
        if (!selected)
        {
            if (doneButton.activeSelf)
            {
                if (hasAnotherSelected)
                    return;
                doneButton.SetActive(false);
                return;
            }
            hasAnotherSelected = false;
        }
    }
}
