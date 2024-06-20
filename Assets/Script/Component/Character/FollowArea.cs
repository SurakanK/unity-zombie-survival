using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FollowArea : MonoBehaviour
{
    private string _targetLayer = "bodyUser";
    private BaseCharacter _owner;
    private BaseCharacter _target;
    public SphereCollider spCollider;

    private void Awake()
    {
        spCollider = GetComponent<SphereCollider>();
    }

    public void Initialize(BaseCharacter ownerArea)
    {
        _owner = ownerArea;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_target != null) return;
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            _target = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
            var enemy = _owner as EnemyNPC;
            enemy.TargetFollow(_target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            var targetExit = other.gameObject.transform.parent.GetComponent<BaseCharacter>();
            if (_target == targetExit)
            {
                _target = null;
                var enemy = _owner as EnemyNPC;
                enemy.TargetFollow(null);
            }
        }
    }
}