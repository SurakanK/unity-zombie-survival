
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ArmyNPC : ArmyBase
{
    [SerializeField] public NavMeshAgent NavMeshAgent;

    private Coroutine _stateCoroutine;
    private BaseCharacter _leader;
    private float _frameDelay = 0.1f;

    public override void Initialize(Factions factions, string jsonData)
    {
        base.Initialize(factions, jsonData);
        NavMeshAgent = GetComponent<NavMeshAgent>();

        MoveSpeed = ArmyData.speed;
        NavMeshAgent.speed = MoveSpeed;
        ChangeState(Idle());
    }

    private void ChangeState(IEnumerator State)
    {
        if (_stateCoroutine != null)
        {
            StopCoroutine(_stateCoroutine);
        }

        _stateCoroutine = StartCoroutine(State);
    }

    public void SetLeader(BaseCharacter leader)
    {
        _leader = leader;
        ChangeState(Follow());
    }

    private IEnumerator Idle()
    {
        yield return null;
    }

    private IEnumerator Follow()
    {
        yield return null;

        while (true)
        {
            yield return null;
            Move();
            yield return new WaitForSeconds(_frameDelay);
        }
    }

    private void Move()
    {
        NavMeshAgent.SetDestination(_leader.transform.position);

        if (NavMeshAgent.velocity.magnitude > 0.01f)
        {
            if (!Animator.GetBool("IsRun"))
            {
                Animator.speed = MoveSpeed / 5;
                Animator.SetBool("IsRun", true);
            }

            Rotation(NavMeshAgent.velocity);
        }
        else
        {
            if (Animator.GetBool("IsRun"))
            {
                Animator.speed = 1f;
                Animator.SetBool("IsRun", false);
            }
        }
    }
}