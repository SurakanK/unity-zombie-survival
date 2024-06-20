using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponModel
{
    public string weaponId;
    public WeaponType type;
    public BaseWeapon baseWeapon;
}

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] private List<WeaponModel> weaponsModel;

    private WeaponCategory _weaponData;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var json = Resources.Load<TextAsset>("weapon.data");
        _weaponData = JsonUtility.FromJson<WeaponCategory>(json.text);
    }

    public BaseWeapon GetGunWeapon(WeaponType type, string weaponId)
    {
        var baseWeapon = GetWeapon(type, weaponId);
        var weaponData = GetWeaponData(type, weaponId);
        baseWeapon.Initialize(weaponData);
        return baseWeapon;
    }

    public BaseWeapon MeleeWeapons(WeaponType type, string weaponId)
    {
        var baseWeapon = GetWeapon(type, weaponId);
        var weaponData = GetWeaponData(type, weaponId);
        var handWeapon = baseWeapon as HandWeapon;
        handWeapon.Initialize(weaponData);
        return baseWeapon;
    }

    private BaseWeapon GetWeapon(WeaponType type, string weaponId)
    {
        var findType = GetWeaponsByType(type);
        var baseWeapon = findType.Find(e => e.weaponId == weaponId).baseWeapon;
        return baseWeapon;
    }

    public WeaponData GetWeaponData(WeaponType type, string weaponId)
    {
        switch (type)
        {
            case WeaponType.Pistols:
                return _weaponData.pistols.Find(e => e.id == weaponId);
            case WeaponType.AssaultRifles:
                return _weaponData.assaultRifles.Find(e => e.id == weaponId);
            case WeaponType.SniperRifles:
                return _weaponData.sniperRifles.Find(e => e.id == weaponId);
            case WeaponType.SubmachineGuns:
                return _weaponData.submachineGuns.Find(e => e.id == weaponId);
            case WeaponType.MachineGuns:
                return _weaponData.machineGuns.Find(e => e.id == weaponId);
            case WeaponType.Shotguns:
                return _weaponData.shotguns.Find(e => e.id == weaponId);
            case WeaponType.MeleeWeapons:
                return _weaponData.meleeWeapons.Find(e => e.id == weaponId);
            case WeaponType.HeavyWeapons:
                return _weaponData.heavyWeapons.Find(e => e.id == weaponId);
            default:
                return null;
        }
    }

    public List<WeaponModel> GetWeaponsByType(WeaponType type)
    {
        List<WeaponModel> result = new List<WeaponModel>();
        foreach (var weapon in weaponsModel)
        {
            if (weapon.type >= type)
            {
                result.Add(weapon);
            }
        }
        return result;
    }
}