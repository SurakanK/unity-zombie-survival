
using UnityEngine;

internal class ShieldAbilitie : BaseAbilitie
{
    private BaseStatus _buff;
    public ShieldAbilitie(BaseCharacter owner, AbilitieData abilitieData) : base(owner, abilitieData)
    {

    }

    public override void Initialized()
    {
        base.Initialized();
        _buff = BaseStatus.CreateStatus(StatusEffectType.Shield, null, abilitieData);
    }

    public override void Apply(BaseCharacter target, Trigger trigger)
    {
        base.Apply(target, trigger);
        _buff.target = target;
        target.statusEffect.AddStatusEffect(_buff);
    }
}