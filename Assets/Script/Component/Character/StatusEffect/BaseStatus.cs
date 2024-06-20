
using UnityEngine;

public abstract class BaseStatus
{
    public StatusEffectType type;
    public BaseCharacter target;
    public bool isBuff;

    protected BaseStatus(StatusEffectType type, BaseCharacter target,  bool isBuff)
    {
        this.isBuff = isBuff;
        this.type = type;
        this.target = target;
    }

    public virtual void Apply()
    {

    }

    public virtual void Remove()
    {

    }

    public static BaseStatus CreateStatus(StatusEffectType type, BaseCharacter target, AbilitieData abilitieData)
    {
        switch (type)
        {
            case StatusEffectType.Slow:
                return new SlowStatus(type, target, abilitieData, false);
            case StatusEffectType.Bleeding:
                return new BleedingStatus(type, target, abilitieData, false);
            case StatusEffectType.Stun:
                return new StunStatus(type, target, abilitieData, false);
            case StatusEffectType.Shield:
                return new ShieldStatus(type, target, abilitieData, true);
            default:
                return null;
        }
    }
}