using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerSelectButtons : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    int id;
    Vector3 startPosition;
    PowerKind powerKind;
    Image image;

    void Start()
    {
        powerKind = UIManager.instance.ToGetKindOfPower(id);
        image = GetComponent<Image>();
        startPosition = transform.position;
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
        if (status)
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

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        UIManager.instance.ToChooseOnlyOne(ref status);
    }
}
