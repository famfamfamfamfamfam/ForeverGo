using System;
using System.Collections.Generic;

public interface ILateUpdateMethodWaitingToRun
{
    void SetUpPosition();
}

public interface IOnAttackable
{
    void OnBeAttacked(PowerKind enemyCurrentPower, AttackState? enemyCurrentAttackState);
}

public interface IAttackStateSettable
{
    void SetAttackState(AttackState? newAttackState);
}

public interface IGetForAttacking
{
    (PowerKind powerKind, AttackState? attackState) GetDataForAttacking();
}

public interface IHitCountForUsingSkillSettable
{
    void SetHitCount();
}

public interface ILoadingInLevel
{
    Dictionary<int, Action> initActionsInLevel { get; }
}