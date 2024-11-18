using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimationContainer : AnimationContainer
{
    public WindAnimationContainer(Animator animator) : base(animator) { }

    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation("riseWind", "RisingWind");
    }
}

public class FireAnimationContainer : AnimationContainer
{
    public FireAnimationContainer(Animator animator) : base(animator) { }

    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation("riseFire", "RisingFire");
    }
}

public class WaterAnimationContainer : AnimationContainer
{
    public WaterAnimationContainer(Animator animator) : base(animator) { }

    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation("riseWater", "RisingWater");
    }
}


