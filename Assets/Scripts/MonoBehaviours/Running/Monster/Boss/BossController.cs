using UnityEngine;

public class BossController : MonsterController
{
    private new void Awake()
    {
        base.Awake();
        Init();
    }

    BossPower power;
    private void Start()
    {
        power = GetComponent<BossPower>();
    }


}
