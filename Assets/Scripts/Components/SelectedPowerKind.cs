public class SelectedPowerKind
{
    public PowerKind[] selectedPowerKinds { get; private set; }
    public SelectedPowerKind()
    {
        selectedPowerKinds = new PowerKind[2];
    }
}
public enum PowerKind
{
    Wind,
    Water,
    Fire
}