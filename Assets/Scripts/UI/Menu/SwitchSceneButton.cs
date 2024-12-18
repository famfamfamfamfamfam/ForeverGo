using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    int destinationSceneIndex;
    public void OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(destinationSceneIndex);
    }
}
