using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IBaseCharacter
{
    public Factions Factions { get; set; }
    public List<BaseAbilitie> Abilities = new List<BaseAbilitie>();

    private float _health;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            HealthUpdate(_health);

            if (_health <= 0)
            {
                Die();
            }
        }
    }

    public float MaxHealth;
    public float Defense;
    public float Critical;
    public float Evasion;
    public float Accuracy;
    public float MoveSpeed;

    public BaseWeapon Weapon { get; set; }
    public AttackArea AttackArea { get; set; }
    public StatusEffect statusEffect { get; set; }

    public bool isDie;

    public bool isCantMove { get; set; }

    private void Awake()
    {
        AllColliderEnabled(true);
    }

    public virtual void Initialize(Factions factions, string jsonData)
    {
        Factions = factions;
        statusEffect = gameObject.AddComponent<StatusEffect>();
    }

    public virtual void ApplyAttack(BaseCharacter target)
    {

    }

    public virtual void RemoveeAttack() { }

    public virtual void Die()
    {
        isDie = true;
    }

    public virtual void HealthUpdate(float health)
    {

    }

    public void RestoreHealth()
    {

    }

    public virtual void SendDamage(BaseCharacter target, BaseCharacter sender)
    {
        if (target == null) return;
        target.TackDamage(target, sender);
        AbilitieTrigger(Trigger.Attack, target);
    }

    public virtual void TackDamage(BaseCharacter target, BaseCharacter sender)
    {
        if (isDie) return;
        var calDamage = CalculateDamage(sender, sender.Weapon.damage);
        var damage = calDamage.damage;
        ApplyDamage(damage, calDamage.isCritical);
        AbilitieTrigger(Trigger.TakeDamage, target);
    }

    public void ApplyDamage(float damage, bool isCritical = false)
    {
        if (!statusEffect.CheckHaveStatus(StatusEffectType.Shield))
        {
            if (damage > 0)
            {
                Health -= damage;
                FloatingText.Instance.FloatingTextString(
                    FloatingType.Damage,
                    damage.ToString(),
                    isCritical,
                    transform,
                    Factions == Factions.Army);

                if (isCritical)
                {
                    StartCoroutine(AddVFX(VFXType.Critical));
                }
            }
            else
            {
                FloatingText.Instance.FloatingTextString("M!", transform, Factions == Factions.Army);
            }
        }
        else
        {
            var shieldStatus = statusEffect.GetStatus(StatusEffectType.Shield) as ShieldStatus;
            shieldStatus.TakeDamage(damage);
            FloatingText.Instance.FloatingTextString("BLOCK", transform, Factions == Factions.Army);
        }
    }

    public virtual void Move(Vector3 velocity) { }

    public void EquipWeapon(BaseWeapon weapon)
    {
        // init attack area
        AttackArea = GetComponentInChildren<AttackArea>();
        AttackArea.SetAttackArea(weapon, this);
        Equiped(weapon);
    }

    private void Equiped(BaseWeapon weapon)
    {
        // add weapon in hand
        if (Factions == Factions.Army)
        {
            var armyBase = this as ArmyBase;
            Instantiate(weapon, armyBase.BodyParts.rightHand.transform);
        }
        else
        {
            Instantiate(weapon, this.transform);
        }

        Weapon = GetComponentInChildren<BaseWeapon>();
        Weapon.Equiped(this);

        AbilitieTrigger(Trigger.Equip);
    }

    public void AllColliderEnabled(bool enabled)
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }
    }

    private (int damage, bool isCritical) CalculateDamage(BaseCharacter sender, float damage)
    {
        var critical = Mathf.Clamp(sender.Critical / 100f, 0f, 1f);
        var evasion = Mathf.Clamp(Evasion / 100f, 0f, 1f);
        float hitChance = Mathf.Clamp(sender.Accuracy / 100f - evasion, 0f, 1f);

        // cal hit damage
        bool isHit = Random.value < hitChance;
        if (!isHit)
        {
            return (0, false);
        }

        // cal critical 
        bool isCritical = Random.value < critical;
        float criticalMultiplier = isCritical ? 2.0f : 1.0f;

        // cal base damage
        float baseDamage = damage * criticalMultiplier;

        // cal damage defense
        float finalDamage = baseDamage - Defense;
        finalDamage = Mathf.Max(finalDamage, 0);

        return ((int)finalDamage, isCritical);
    }

    public void AbilitieTrigger(Trigger trigger, BaseCharacter target = null)
    {
        var filter = Abilities.Where(e => e.abilitieData.Trigger.Any(t => t == trigger)).ToList();
        for (int i = 0; i < filter.Count; i++)
        {
            var abilitis = filter[i];
            if (target == null && abilitis.abilitieData.action.Target == TargetType.Self)
            {
                target = this;
            }

            abilitis.Apply(target, trigger);
        }
    }

    private IEnumerator AddVFX(VFXType type)
    {
        yield return null;
        var vfx = Factory.Instance.vFXFactory.CharacterVFXPool[VFXType.Critical].Get();
        vfx.transform.parent = transform;
        vfx.transform.localPosition = Vector3.zero;
        vfx.Play();

        while (vfx.isPlaying)
        {
            yield return null;
        }

        Factory.Instance.vFXFactory.ReleaseVFX(type, vfx);
    }
}