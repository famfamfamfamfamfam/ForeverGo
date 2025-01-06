using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int currentLevel { get; private set; }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public void InitInLevel(ILoadingInLevel initPlace)
    {
        initPlace.initActionsInLevel[currentLevel].Invoke();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetLevel(1);
    }


}
