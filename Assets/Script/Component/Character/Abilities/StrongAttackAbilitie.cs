
using UnityEngine;

internal class StrongAttackAbilitie : BaseAbilitie
{
    public StrongAttackAbilitie(BaseCharacter owner, AbilitieData abilitieData) : base(owner, abilitieData)
    {

    }

    public override void Initialized()
    {
        base.Initialized();
    }

    public override void Apply(BaseCharacter target, Trigger trigger)
    {
        base.Apply(target, trigger);
        target.Weapon.damage += abilitieData.action.attack;
        target.Critical = 80;
    }
}