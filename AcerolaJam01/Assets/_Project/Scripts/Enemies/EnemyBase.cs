using UnityEditor;
public enum BodyParts {Arms, Legs, Head, Torso};

public abstract class EnemyBase{
    public abstract void InitState(EnemyManager enemy);
    public abstract void TakeDamage(EnemyManager enemy, float damage, BodyParts part);
    public abstract void Attack(EnemyManager enemy, float damage, BodyParts part);
}
