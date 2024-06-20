using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimEvent : MonoBehaviour
{
    public UnityAction OnAttackEvent;
    public void Attack()
    {
        OnAttackEvent?.Invoke();
    }
}