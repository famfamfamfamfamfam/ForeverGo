public abstract class GameStateBase
{
    protected GameManager gameManager;
    public GameStateBase(GameManager manager)
    {
        gameManager = manager;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
