public interface ILateUpdateMethodWaitingToRun
{
    void SetUpPosition();
}

public interface IOnAttackable
{
    void OnBeAttacked(PowerKind enemyCurrentPower);
    //void OnNormalAttack(PowerKind enemyCurrentPower, bool enemyIsInNaturePowerState);
    //void OnSuperAttack(PowerKind enemyCurrentPower);
    //void OnUniqueSkill(PowerKind enemyCurrentPower);
    //react, tru mau dua tren tham so
}