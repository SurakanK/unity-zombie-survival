using System;
using System.Collections.Generic;

[Serializable]
public class WeaponData
{
    public string id;
    public float damage;
    public float attackRate;
    public float bulletSpeed;
    public int bulletType;
    public float attackArea;
}

[Serializable]
public class WeaponCategory
{
    public List<WeaponData> pistols;
    public List<WeaponData> assaultRifles;
    public List<WeaponData> sniperRifles;
    public List<WeaponData> submachineGuns;
    public List<WeaponData> machineGuns;
    public List<WeaponData> shotguns;
    public List<WeaponData> heavyWeapons;
    public List<WeaponData> meleeWeapons;
}