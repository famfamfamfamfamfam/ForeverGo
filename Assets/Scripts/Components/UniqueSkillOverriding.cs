using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimationContainer : AnimationContainer
{
    int risingWindState;
    public WindAnimationContainer(Animator animator, int risingWindStateHash) : base(animator)
    {
        risingWindState = risingWindStateHash;
    }

    int riseWind = Animator.StringToHash("riseWind");
    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation(riseWind, risingWindState);
    }
}

public class FireAnimationContainer : AnimationContainer
{
    int risingFireState;
    public FireAnimationContainer(Animator animator, int risingFireStateHash) : base(animator)
    {
        risingFireState = risingFireStateHash;
    }

    int riseFire = Animator.StringToHash("riseFire");
    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation(riseFire, risingFireState);
    }
}

public class WaterAnimationContainer : AnimationContainer
{
    int risingWaterState;
    public WaterAnimationContainer(Animator animator, int risingWaterStateHash) : base(animator)
    {
        risingWaterState = risingWaterStateHash;
    }

    int riseWater = Animator.StringToHash("riseWater");
    public override void UniqueSkill()
    {
        TurnOnTemporaryAnimation(riseWater, risingWaterState);
    }
}


