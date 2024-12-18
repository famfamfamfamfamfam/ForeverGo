using UnityEngine;
using UnityEngine.EventSystems;

public class OnSwitchingPauseState : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.PauseOrUnpauseGame();
    }
}
