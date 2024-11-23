using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerSelectButtons : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    int id;
    PlayerPowerKind powerKind;

    void Start()
    {
        powerKind = UIManager.instance.ToGetKindOfPower(id);
    }

    bool turn, status;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!status && UIManager.instance.DoneButtonIsActived())
            return;
        turn = !turn;
        UIManager.instance.ToReceiveSelection(turn, ref status, gameObject);
        UIManager.instance.ToUnselect(turn, ref status, gameObject);
    }

    void OnDisable()
    {
        if (status)
            UIManager.instance.ToSetUpTheSelectedPower(powerKind);
        else
            UIManager.instance.ToSetUpTheUnselectedPower(powerKind);
    }
}
