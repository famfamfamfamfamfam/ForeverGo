using UnityEngine;

public class BossController : MonoBehaviour
{
    BossPower power;
    private void Start()
    {
        power = GetComponent<BossPower>();
    }


}
