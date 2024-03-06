using UnityEditor;
public enum BodyParts {Arms, Legs, Head, Torso};

public abstract class EnemyBase{
    public abstract void InitState(EnemyManager enemy, InfoEnemies info);
    public abstract void TakeDamage(EnemyManager enemy, InfoEnemies info, float damage, BodyParts part);
    public abstract void Attack(EnemyManager enemy, InfoEnemies info, float damage, BodyParts part);
}
