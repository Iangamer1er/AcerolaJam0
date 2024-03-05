public abstract class EnemyBase{
    public abstract void InitState(EnemyManager enemy, InfoEnemies info);
    public abstract void TakeDamage(EnemyManager enemy, InfoEnemies info);
    public abstract void Attack(EnemyManager enemy, InfoEnemies info);
}
