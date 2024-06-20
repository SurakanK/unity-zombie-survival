using UnityEngine;

public class HandWeapon : BaseWeapon
{
    public override void Initialize(WeaponData weaponData)
    {
        base.Initialize(weaponData);
    }


    public override void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
        Target = target;
        Attack(Target);
    }

    public override void OnTriggerExit(Collider other)
    {
        var target = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
        if (target == Target)
        {
            Target = null;
            Owner.RemoveeAttack();
        }
    }
}