using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [HideInInspector] public string weaponId;
    [HideInInspector] public float damage;
    [HideInInspector] public float attackRate;
    [HideInInspector] public float AttackArea;
    [HideInInspector] public BaseCharacter Target;
    [HideInInspector] public BaseCharacter Owner;

    public virtual void Initialize(WeaponData weaponData)
    {
        weaponId = weaponData.id;
        damage = weaponData.damage;
        attackRate = weaponData.attackRate;
        AttackArea = weaponData.attackArea;
    }

    public virtual void Equiped(BaseCharacter ownerWeapon)
    {
        Owner = ownerWeapon;
        OnEvent();
    }

    public virtual void Attack(BaseCharacter target)
    {
        Owner.ApplyAttack(target);
    }

    private void OnEvent()
    {
        Owner.AttackArea.OnTriggerEnterEvent += OnTriggerEnter;
        Owner.AttackArea.OnTriggerExitEvent += OnTriggerExit;
    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnTriggerExit(Collider other)
    {

    }
}