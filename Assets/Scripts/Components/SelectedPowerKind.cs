using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPowerKind
{
    public PowerKind[] selectedPowerKind {  get; private set; }
    public SelectedPowerKind()
    {
        selectedPowerKind = new PowerKind[2];
    }
}
