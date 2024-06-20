using System;
using System.Collections.Generic;

[Serializable]
public class AbilitiesModel
{
    public List<AbilitieData> abilitis;
}

[Serializable]
public class AbilitieData
{
    public string id;
    public string[] trigger;
    public AbilitieAction action;

    public AbilitieType type
    {
        get
        {
            return Enum.Parse<AbilitieType>(id, true);
        }
    }

    public Trigger[] Trigger
    {
        get
        {
            var t = new Trigger[trigger.Length];
            for (int i = 0; i < trigger.Length; i++)
            {
                t[i] = Enum.Parse<Trigger>(trigger[i], true);
            }
            return t;
        }
    }
}

[Serializable]
public class AbilitieAction
{
    public string id;
    public float dulation;
    public float speed;
    public string target;
    public int spawnAmount;
    public float cooldown;
    public float attack;
    public float defense;

    public TargetType Target
    {
        get
        {
            return Enum.Parse<TargetType>(target, true);
        }
    }
}
