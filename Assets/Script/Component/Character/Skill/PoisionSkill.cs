using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PoisionSkill : BaseSkill
{
    [SerializeField] private ParticleSystem _targetingFx;
    [SerializeField] private ParticleSystem _skillFx;

    private BaseStatus _debuff;
    private SphereCollider _sp;

    private void Awake()
    {
        _sp = GetComponent<SphereCollider>();
    }

    public override void Spawn(BaseStatus baseStatus)
    {
        base.Spawn(baseStatus);
        _debuff = baseStatus;
        StartCoroutine(SpawnSkill());
    }

    private IEnumerator SpawnSkill()
    {
        yield return null;
        _skillFx.Stop();
        _sp.enabled = false;

        yield return new WaitForSeconds(2f);
        _skillFx.Play();
        _sp.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("bodyUser"))
        {
            var target = other.transform.parent.GetComponent<BaseCharacter>();
            _debuff.target = target;
            target.statusEffect.AddStatusEffect(_debuff);
        }
    }
}