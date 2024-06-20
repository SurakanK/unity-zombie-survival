using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BulletGun : MonoBehaviour
{
    [SerializeField] public ParticleSystem bullet;
    [SerializeField] public ParticleSystem hitFxPrefab;
    [SerializeField] public ParticleSystem fireFXPrefab;

    private float _speed;
    private float _damage;
    private GunWeapon _gunWeapon;
    private Collider _collider;

    private float _curTime = 0;
    private float _timelife = 2f;

    private ParticleSystem _hitFx;

    private Coroutine _hitFxCoroutine;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Initialize(GunWeapon gunWeapon, float speed, float damage, Factions factions)
    {
        _gunWeapon = gunWeapon;
        _speed = speed;
        _damage = damage;
        _curTime = 0;

        var layerName = factions == Factions.Army ? "bulletArmy" : "";
        gameObject.layer = LayerMask.NameToLayer(layerName);

        _collider.enabled = true;
        bullet.gameObject.SetActive(true);
    }

    private void Update()
    {
        _curTime += Time.deltaTime;

        if (_speed != 0)
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
        }

        if (_curTime >= _timelife)
        {
            if (_gunWeapon.bulletPool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                _gunWeapon.bulletPool.Release(this);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var targetTake = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
        if (targetTake == null) return;

        _speed = 0;
        _collider.enabled = false;

        ContactPoint contact = other.contacts[0];

        if (_hitFxCoroutine == null)
        {
            _hitFxCoroutine = StartCoroutine(AddHitFx(contact));
            _gunWeapon.Owner.SendDamage(targetTake, _gunWeapon.Owner);
        }

        bullet.gameObject.SetActive(false);
    }

    private IEnumerator AddHitFx(ContactPoint contact)
    {
        yield return null;

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        _hitFx = _gunWeapon.hitFxPool.Get();
        _hitFx.transform.position = pos;
        _hitFx.transform.rotation = rot;

        yield return new WaitForSeconds(_hitFx.main.duration);

        if (_gunWeapon.hitFxPool == null)
        {
            Destroy(_hitFx.gameObject);
        }
        else
        {
            _gunWeapon.hitFxPool.Release(_hitFx);
        }

        if (_gunWeapon.bulletPool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            _gunWeapon.bulletPool.Release(this);
        }

        StopCoroutine(_hitFxCoroutine);
        _hitFxCoroutine = null;
    }
}