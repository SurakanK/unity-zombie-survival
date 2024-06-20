using System.Collections;
using UnityEngine;

public abstract class BaseAbilitie
{
    public BaseCharacter owner;
    public AbilitieData abilitieData;

    public static BaseAbilitie Create(BaseCharacter owner, AbilitieData abilitieData)
    {
        switch (abilitieData.type)
        {
            case AbilitieType.SlowsArmy:
                return new SlowAbilitie(owner, abilitieData);
            case AbilitieType.Bleeding:
                return new BleedingAbilitie(owner, abilitieData);
            case AbilitieType.Runner:
                return new RunnerAbilitie(owner, abilitieData);
            case AbilitieType.Poisons:
                return new PoisonsAbilitie(owner, abilitieData);
            case AbilitieType.Stun:
                return new StunAbilitie(owner, abilitieData);
            case AbilitieType.StrongAttack:
                return new StrongAttackAbilitie(owner, abilitieData);
            case AbilitieType.Shield:
                return new ShieldAbilitie(owner, abilitieData);
            default:
                return null;
        }
    }

    public BaseAbilitie(BaseCharacter owner, AbilitieData abilitieData)
    {
        this.owner = owner;
        this.abilitieData = abilitieData;
        Initialized();
    }

    public virtual void Initialized()
    {

    }

    public virtual void Apply(BaseCharacter target, Trigger trigger)
    {

    }
}