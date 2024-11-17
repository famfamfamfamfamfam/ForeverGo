using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimationContainer : AnimationContainer
{
    public WindAnimationContainer(Animator animator, GameObject weapon) : base(animator, weapon) { }

    public override void UniqueSkill()
    {
        base.UniqueSkill();
        TurnOnTemporaryAnimation("riseWind", "RisingWind");
    }
}

public class FireAnimationContainer : AnimationContainer
{
    public FireAnimationContainer(Animator animator, GameObject weapon) : base(animator, weapon) { }

    public override void UniqueSkill()
    {
        base.UniqueSkill();
        TurnOnTemporaryAnimation("riseFire", "RisingFire");
    }
}

public class WaterAnimationContainer : AnimationContainer
{
    public WaterAnimationContainer(Animator animator, GameObject weapon) : base(animator, weapon) { }

    public override void UniqueSkill()
    {
        base.UniqueSkill();
        TurnOnTemporaryAnimation("riseWater", "RisingWater");
    }
}


