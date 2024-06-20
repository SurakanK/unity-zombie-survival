using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GunWeapon : BaseWeapon
{
    [SerializeField] public GameObject firePoint;
    [SerializeField] public BulletGun BulletGun;

    [HideInInspector] public int bulletType;
    [HideInInspector] public float bulletSpeed;

    private ObjectPool<ParticleSystem> _fireFxPool;
    public ObjectPool<ParticleSystem> hitFxPool;
    public ObjectPool<BulletGun> bulletPool;
    private List<BaseCharacter> _allTarget = new List<BaseCharacter>();

    private ArmyBase _owner;

    private float _curTime = 0;

    private void Awake()
    {
        _fireFxPool = new ObjectPool<ParticleSystem>(CreateFireFxPool, GetFireFxPool, ReleaseFireFxPool, DestroyFireFxPool);
        hitFxPool = new ObjectPool<ParticleSystem>(CreateHitFxPool, GetHitFxPool, ReleaseHitFxPool, DestroyHitFxPool);
        bulletPool = new ObjectPool<BulletGun>(CreateBulletPool, GetBulletPool, ReleaseBulletPool, DestroyBulletPool);
    }

    public override void Initialize(WeaponData weaponData)
    {
        base.Initialize(weaponData);
        bulletType = weaponData.bulletType;
        bulletSpeed = weaponData.bulletSpeed;
    }

    public override void Equiped(BaseCharacter ownerWeapon)
    {
        base.Equiped(ownerWeapon);
        _owner = ownerWeapon as ArmyBase;
    }

    public override void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
        _allTarget.Add(target);
    }

    public override void OnTriggerExit(Collider other)
    {
        var target = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
        _allTarget.Remove(target);
    }

    public BaseCharacter GetTarget()
    {
        _allTarget.RemoveAll(character => character.isDie);

        if (_allTarget.Count > 0)
        {
            _allTarget.Sort(delegate (BaseCharacter a, BaseCharacter b)
            {
                return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
            });

            return _allTarget[0];
        }

        return null;
    }

    private void Update()
    {
        if (Target == null)
        {
            var newTarget = GetTarget();
            if (newTarget != null)
            {
                SetTarget(newTarget);
            }
            return;
        };

        if (OutOfRange() || Target.isDie)
        {
            RemoveTarget();
            return;
        }

        _curTime += Time.deltaTime;
        if (_curTime >= attackRate)
        {
            Attack(Target);
            _curTime = 0;
        }

        _owner.RotateToTarget(Target);
    }

    private void SetTarget(BaseCharacter newTarget)
    {
        Target = newTarget;
        _owner.ShootAnim(true);
    }

    private void RemoveTarget()
    {
        Target = null;
        _owner.ShootAnim(false);
    }

    private bool OutOfRange()
    {
        return Vector3.Distance(Target.transform.position, Owner.transform.position) > AttackArea;
    }

    private void OnDestroy()
    {
        _fireFxPool.Dispose();
        hitFxPool.Dispose();
        bulletPool.Dispose();

        _fireFxPool = null;
        hitFxPool = null;
        bulletPool = null;
    }

    private ParticleSystem CreateFireFxPool()
    {
        return Instantiate(BulletGun.fireFXPrefab, firePoint.transform.position, Quaternion.identity);
    }

    private void ReleaseFireFxPool(ParticleSystem fx)
    {
        fx.Stop();
        fx.gameObject.SetActive(false);
    }

    private void GetFireFxPool(ParticleSystem fx)
    {
        fx.transform.forward = gameObject.transform.forward;
        fx.transform.parent = firePoint.transform;
        fx.Play();

        fx.gameObject.SetActive(true);
    }

    private void DestroyFireFxPool(ParticleSystem fx)
    {
        if (fx == null) return;
        Destroy(fx.gameObject);
    }

    private ParticleSystem CreateHitFxPool()
    {
        return Instantiate(BulletGun.hitFxPrefab);
    }

    private void GetHitFxPool(ParticleSystem fx)
    {
        fx.gameObject.SetActive(true);
    }

    private void ReleaseHitFxPool(ParticleSystem fx)
    {
        fx.gameObject.SetActive(false);
    }

    private void DestroyHitFxPool(ParticleSystem fx)
    {
        if (fx == null) return;
        Destroy(fx.gameObject);
    }

    private BulletGun CreateBulletPool()
    {
        return Instantiate(BulletGun, firePoint.transform.position, Quaternion.identity);
    }

    private void GetBulletPool(BulletGun bullet)
    {
        bullet.transform.position = firePoint.transform.position;
        bullet.gameObject.SetActive(true);
    }

    private void ReleaseBulletPool(BulletGun bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void DestroyBulletPool(BulletGun bullet)
    {
        if (bullet == null) return;
        Destroy(bullet.gameObject);
    }

    public override void Attack(BaseCharacter target)
    {
        base.Attack(target);
        SpawnBullet();
        StartCoroutine(AddFirePointFx());
    }

    private IEnumerator AddFirePointFx()
    {
        yield return null;
        var firePointFx = _fireFxPool.Get();
        yield return new WaitForSeconds(firePointFx.main.duration);
        _fireFxPool.Release(firePointFx);
    }

    private void SpawnBullet()
    {
        var bullet = bulletPool.Get();
        Vector3 targetPostition = new Vector3(Target.transform.position.x, firePoint.transform.position.y, Target.transform.position.z);
        bullet.Initialize(this, bulletSpeed, damage, Owner.Factions);
        bullet.transform.LookAt(targetPostition);
    }
}
