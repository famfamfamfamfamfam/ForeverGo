using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerSelectButtons : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    int id;
    PowerKind powerKind;
    Image image;

    void Start()
    {
        powerKind = UIManager.instance.ToGetKindOfPower(id);
        image = GetComponent<Image>();
    }

    bool turn, status;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!status && UIManager.instance.DoneButtonIsActived())
            return;
        turn = !turn;
        UIManager.instance.ToReceiveSelection(turn, ref status, gameObject);
        UIManager.instance.ToUnselect(turn, ref status, gameObject);
        UIManager.instance.ToDisplayQuitButton();
        if (!status)
            image.color = Color.white;
        else
            image.color = Color.gray;
    }

    void OnDisable()
    {
        if (status)
            UIManager.instance.ToSetUpTheSelectedPower(powerKind);
        else
            UIManager.instance.ToSetUpTheUnselectedPower(powerKind);
    }
}
