using System.Collections;
using UnityEngine;

public class StunStatus : BaseStatus
{
    private AbilitieData _abilitieData;

    public StunStatus(StatusEffectType type, BaseCharacter target, AbilitieData abilitieData, bool isBuff) : base(type, target, isBuff)
    {
        _abilitieData = abilitieData;
    }

    public override void Apply()
    {
        base.Apply();
        target.StartCoroutine(ApplyEffect());
    }

    private IEnumerator ApplyEffect()
    {
        yield return null;
        target.isCantMove = true;

        // stun user
        if (target.Factions == Factions.Army)
        {
            var army = target as ArmyUser;
            army.Move(Vector3.zero);
        }

        var vfx = Factory.Instance.vFXFactory.StatusVFXPool[type].Get();
        vfx.transform.parent = target.transform;
        vfx.transform.localPosition = Vector3.zero;
        vfx.Play();

        yield return new WaitForSeconds(_abilitieData.action.dulation);
        target.isCantMove = false;

        vfx.Stop();
        target.statusEffect.RemoveStatusEffect(this);

        while (vfx.isPlaying)
        {
            yield return null;
        }

        Factory.Instance.vFXFactory.ReleaseVFX(type, vfx);
    }
}