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

public interface IAttackStateGettable
{
    AttackState? GetAttackState();
}

public interface IPowerKindGettable
{
    PowerKind GetPowerKind();
}