using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public AbilitieType type;

    public virtual void Initialized(AbilitieData abilitieData = null)
    {

    }
    
    public virtual void Spawn()
    {

    }

    public virtual void Spawn(BaseStatus baseStatus)
    {

    }
}