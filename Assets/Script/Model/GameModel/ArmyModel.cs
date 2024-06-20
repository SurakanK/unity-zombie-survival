
using System;
using System.Collections.Generic;

[Serializable]
public class ArmysDataModel
{
    public List<ArmyData> armys;
}

[Serializable]
public class ArmyData
{
    private string _id;

    public string id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
            type = Enum.Parse<ArmyType>(_id, true);
        }
    }

    public string name;
    public float hp;
    public float speed;
    public float defense;
    public float critical;
    public float evasion;
    public float accuracy;
    public string[] appropriate_weapons;
    public string[] abilities;
    public ArmyType type;
}