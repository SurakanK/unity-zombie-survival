using System.Collections;
using UnityEngine;

internal class BleedingStatus : BaseStatus
{
    private float _damage;
    private AbilitieData _abilitieData;

    public BleedingStatus(StatusEffectType type, BaseCharacter target, AbilitieData abilitieData, bool isBuff) : base(type, target, isBuff)
    {
        _abilitieData = abilitieData;
    }

    public override void Apply()
    {
        base.Apply();
        _damage = (int)(target.MaxHealth * 0.02f);
        target.StartCoroutine(ApplyEffect());
    }

    private IEnumerator ApplyEffect()
    {
        yield return null;

        var vfx = Factory.Instance.vFXFactory.StatusVFXPool[type].Get();
        vfx.transform.parent = target.transform;
        vfx.transform.localPosition = Vector3.zero;
        vfx.Play();

        var count = 0;
        while (count < _abilitieData.action.dulation * 2)
        {
            target.ApplyDamage(_damage);
            yield return new WaitForSeconds(0.5f);
            count++;
        }

        vfx.Stop();
        target.statusEffect.RemoveStatusEffect(this);

        while (vfx.isPlaying)
        {
            yield return null;
        }

        Factory.Instance.vFXFactory.ReleaseVFX(type, vfx);
    }
}