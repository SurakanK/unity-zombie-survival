using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillArea : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private string _targetLayer;
    private BaseCharacter _owner;
    public UnityAction<Collider> OnTriggerEnterEvent;
    public UnityAction<Collider> OnTriggerExitEvent;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void SetSkillArea(BaseWeapon baseWeapon, BaseCharacter owner)
    {
        sphereCollider.radius = baseWeapon.AttackArea;
        SetSkillArea(owner);
    }

    public void SetSkillArea(BaseCharacter owner)
    {
        _owner = owner;
        _targetLayer = _owner.Factions == Factions.Army ? "bodyEnemy" : "bodyUser";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            OnTriggerEnterEvent?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            OnTriggerExitEvent?.Invoke(other);
        }
    }
}