public interface ILateUpdateMethodWaitingToRun
{
    void SetUpPosition();
}

public interface IOnAttackable
{
    void OnBeAttacked(PowerKind enemyCurrentPower);
}

public interface IAttackStateSettable
{
    void SetAttackState(AttackState? newAttackState);
}

public interface IPowerKindGettable
{
    PowerKind GetPowerKind();
}