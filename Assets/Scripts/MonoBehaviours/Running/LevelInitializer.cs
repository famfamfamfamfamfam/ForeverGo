using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private void Start()
    {
        LevelManager.instance.InitInLevel(GameManager.instance);
        LevelManager.instance.InitInLevel(GamePlayUIManager.instance);
        LevelManager.instance.InitInLevel(MonstersManager.instance);
    }
}
