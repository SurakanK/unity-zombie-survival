using System.Collections;
using UnityEngine;

public class ShieldStatus : BaseStatus
{
    private AbilitieData _abilitieData;
    private float _damageDefense;
    private ParticleSystem _shieldFxLoop;

    public ShieldStatus(StatusEffectType type, BaseCharacter target, AbilitieData abilitieData, bool isBuff) : base(type, target, isBuff)
    {
        _abilitieData = abilitieData;
    }

    public override void Apply()
    {
        base.Apply();
        _damageDefense = _abilitieData.action.defense;
        _shieldFxLoop = Factory.Instance.vFXFactory.CharacterVFXPool[VFXType.ShieldEnemy_Loop].Get();
        _shieldFxLoop.transform.parent = target.transform;
        _shieldFxLoop.transform.localPosition = Vector3.zero;
        _shieldFxLoop.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _shieldFxLoop.Play();
    }

    public void TakeDamage(float damage)
    {
        _damageDefense -= damage;
        if (_damageDefense <= 0)
        {
            target.statusEffect.RemoveStatusEffect(this);
            target.StartCoroutine(ShieldBroken());
        }
    }

    private IEnumerator ShieldBroken()
    {
        // release shield loop
        Factory.Instance.vFXFactory.ReleaseVFX(VFXType.ShieldEnemy_Loop, _shieldFxLoop);

        var shieldEnd = Factory.Instance.vFXFactory.CharacterVFXPool[VFXType.ShieldEnemy_End].Get();
        shieldEnd.transform.parent = target.transform;
        shieldEnd.transform.localPosition = Vector3.zero;
        shieldEnd.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        shieldEnd.Play();

        while (shieldEnd.isPlaying)
        {
            yield return null;
        }

        Factory.Instance.vFXFactory.ReleaseVFX(VFXType.ShieldEnemy_End, shieldEnd);
    }
}