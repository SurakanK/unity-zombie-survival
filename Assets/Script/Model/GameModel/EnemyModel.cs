
using System;
using System.Collections.Generic;

[Serializable]
public class EnemysDataModel
{
    public List<EnemyData> enemies;
}

[Serializable]
public class EnemyData
{
    public string id;
    public string name;
    public string difficulty;
    public float hp;
    public float speed;
    public float defense;
    public float critical;
    public float evasion;
    public float accuracy;
    public string[] abilities;

    public EnemyType type
    {
        get
        {
            return Enum.Parse<EnemyType>(id, true);
        }
    }
}