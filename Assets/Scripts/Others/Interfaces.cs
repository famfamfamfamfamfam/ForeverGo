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
    void SetAttackState(AttackSate? newAttackState);
}