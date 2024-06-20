
using UnityEngine;

internal class StunAbilitie : BaseAbilitie
{
    private BaseStatus _debuff;
    public StunAbilitie(BaseCharacter owner, AbilitieData abilitieData) : base(owner, abilitieData)
    {

    }

    public override void Initialized()
    {
        base.Initialized();
        _debuff = BaseStatus.CreateStatus(StatusEffectType.Stun, null, abilitieData);
    }

    public override void Apply(BaseCharacter target, Trigger trigger)
    {
        base.Apply(target, trigger);

        _debuff.target = target;
        target.statusEffect.AddStatusEffect(_debuff);
    }
}