using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private List<BaseStatus> _statusEffects = new List<BaseStatus>();

    public void AddStatusEffect(BaseStatus baseStatus)
    {
        if (!CheckHaveStatus(baseStatus))
        {
            baseStatus.Apply();
            _statusEffects.Add(baseStatus);
            // show icon status effect for user

        }
    }

    public void RemoveStatusEffect(BaseStatus baseStatus)
    {
        baseStatus.Remove();
        _statusEffects.Remove(baseStatus);
        // remove icon status effect for user
    }

    public bool CheckHaveStatus(BaseStatus baseStatus)
    {
        for (int i = 0; i < _statusEffects.Count; i++)
        {
            var status = _statusEffects[i];
            if (status.type == baseStatus.type)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckHaveStatus(StatusEffectType type)
    {
        for (int i = 0; i < _statusEffects.Count; i++)
        {
            var status = _statusEffects[i];
            if (status.type == type)
            {
                return true;
            }
        }
        return false;
    }

    public BaseStatus GetStatus(StatusEffectType type)
    {
        return _statusEffects.Find(e => e.type == type);
    }
}