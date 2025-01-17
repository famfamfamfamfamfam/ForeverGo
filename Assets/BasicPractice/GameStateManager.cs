public class GameStateManager
{
    GameStateBase state;
    public GameStateBase _state
    {
        get => state;
        set
        {
            state?.Exit();
            state = value;
            state.Enter();
        }
    }
}
