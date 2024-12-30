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
        powerKind = MenuUIManager.instance.ToGetKindOfPower(id);
        image = GetComponent<Image>();
        startPosition = transform.position;
    }

    bool turn, state;
    public void OnPointerDown(PointerEventData eventData)
    {
        if ((!state && MenuUIManager.instance.DoneButtonIsActived()) || PlayerSelectionData.Instance.onlyOneMode)
            return;
        turn = !turn;
        MenuUIManager.instance.ToReceiveSelection(turn, ref state, gameObject);
        MenuUIManager.instance.ToUnselect(turn, ref state, gameObject);
        MenuUIManager.instance.ToDisplayQuitButton();
        if (state)
            image.color = Color.white;
        else
            image.color = Color.gray;
    }

    void OnDisable()
    {
        if (state)
            MenuUIManager.instance.ToSetUpTheSelectedPower(powerKind);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        MenuUIManager.instance.ToChooseOnlyOne();
    }
}
