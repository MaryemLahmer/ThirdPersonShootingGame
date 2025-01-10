// Interface for anything that can be damaged
public interface IDamageable
{
    void ApplyDamage(int damage);
}
// Enum to identify bullet owner
public enum BulletOwner
{
    Player,
    Enemy
}