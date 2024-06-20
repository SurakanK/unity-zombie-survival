using UnityEngine;

public interface IBaseCharacter
{
    public Factions Factions { get; set; }
    public BaseWeapon Weapon { get; set; }
    public AttackArea AttackArea { get; set; }

    public void Move(Vector3 velocity);
    public void EquipWeapon(BaseWeapon weapon);
    public void ApplyAttack(BaseCharacter target);
    public void SendDamage(BaseCharacter target, BaseCharacter sender);
    public void TackDamage(BaseCharacter target, BaseCharacter sender);
    public void Die();
    public void RestoreHealth();
}