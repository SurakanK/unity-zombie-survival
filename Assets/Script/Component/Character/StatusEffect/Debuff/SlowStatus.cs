using System.Collections;
using UnityEngine;

internal class SlowStatus : BaseStatus
{
    private AbilitieData _abilitieData;
    public SlowStatus(StatusEffectType type, BaseCharacter target, AbilitieData abilitieData, bool isBuff) : base(type, target, isBuff)
    {
        _abilitieData = abilitieData;
    }

    public override void Apply()
    {
        base.Apply();
        target.StartCoroutine(ApplyEffect());
    }

    public override void Remove()
    {
        base.Remove();
    }

    private IEnumerator ApplyEffect()
    {
        yield return null;

        var vfx = Factory.Instance.vFXFactory.StatusVFXPool[type].Get();
        vfx.transform.parent = target.transform;
        vfx.transform.localPosition = Vector3.zero;
        vfx.Play();

        var baseSpeed = target.MoveSpeed;
        target.MoveSpeed = target.MoveSpeed / 2;

        yield return new WaitForSeconds(_abilitieData.action.dulation);

        vfx.Stop();
        target.MoveSpeed = baseSpeed;
        target.statusEffect.RemoveStatusEffect(this);

        while (vfx.isPlaying)
        {
            yield return null;
        }

        Factory.Instance.vFXFactory.ReleaseVFX(type, vfx);
    }
}