using System.Collections;
using FSG.MeshAnimator;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : BaseCharacter
{
    public enum State { Patrol, Follow, Chase, Attack, Die }

    private Coroutine _stateCoroutine;
    private State _curState;

    private MeshAnimatorBase _meshAnimator;
    private BaseCharacter _targetFollow;
    private EnemyAnimEvent _enemyAnimEvent;


    public EnemyData enemyData { get; set; }
    public FollowArea FollowArea { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }
    public EnemyType enemyType { get; set; }
    public BaseCharacter targetAttack { get; set; }
    public BaseCharacter targetChase { get; set; }

    public float rangeOfChase = 20f;
    public float frameDelay = 0.5f;

    private bool _attacking = false;

    public override void Initialize(Factions factions, string jsonData)
    {
        base.Initialize(factions, jsonData);

        FollowArea = GetComponentInChildren<FollowArea>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _meshAnimator = GetComponentInChildren<MeshAnimatorBase>();
        _enemyAnimEvent = GetComponentInChildren<EnemyAnimEvent>();
        enemyData = JsonUtility.FromJson<EnemyData>(jsonData);

        Defense = enemyData.defense;
        Critical = enemyData.critical;
        Evasion = enemyData.evasion;
        Accuracy = enemyData.accuracy;

        FollowArea.Initialize(this);

        // create abilitie 
        Factory.Instance.abilitieFactory.CreateAbilitie(this, enemyData.abilities);
    }

    public void StartEnemy()
    {
        MaxHealth = enemyData.hp;
        Health = enemyData.hp;
        MoveSpeed = enemyData.speed;
        enemyType = enemyData.type;
        targetAttack = null;
        isDie = false;
        targetChase = null;

        _targetFollow = null;

        OnEvent();
        StatrState();

        AbilitieTrigger(Trigger.Start);
    }

    private void OnEvent()
    {
        if (_enemyAnimEvent)
        {
            _enemyAnimEvent.OnAttackEvent += OnAttackEvent;
        }
    }
    private void OnDisable()
    {
        _enemyAnimEvent.OnAttackEvent -= OnAttackEvent;
        StopAllCoroutines();
    }

    public virtual void StatrState()
    {
        var user = UserMain.Instance.userArmy;
        if (Random.value < 0.05f &&
            Vector3.Distance(transform.position, user.transform.position) > 10)
        {
            rangeOfChase = 50;
            targetChase = user;
            ChangeState(Chase(), State.Chase);
        }
        else
        {
            ChangeState(Patrol(), State.Patrol);
        }
    }

    public void ChangeState(IEnumerator State, State stateName)
    {
        if (_stateCoroutine != null)
        {
            StopCoroutine(_stateCoroutine);
            _stateCoroutine = null;
        }

        _stateCoroutine = StartCoroutine(State);
        _curState = stateName;
    }

    public IEnumerator Patrol()
    {
        yield return null;
        // Debug.Log("Patrol" + GetInstanceID().ToString());

        while (true)
        {
            var randomState = Random.Range(0, 10);
            var timePatrol = Random.Range(3f, 15f);

            if (randomState >= 5) //walk
            {

                Vector3 newPosition = new Vector3(
                    transform.position.x + Random.Range(-10, 10),
                    transform.position.y,
                    transform.position.z + Random.Range(-10, 10)
                );

                WalkAnim();
                NavMeshAgent.SetDestination(newPosition);

                float timeWalk = 0;
                while (true)
                {
                    if (Vector3.Distance(transform.position, newPosition) < 2f)
                    {
                        NavMeshAgent.SetDestination(transform.position);
                        break;
                    }

                    timeWalk += Time.deltaTime;
                    if (timeWalk >= 15f)
                    {
                        break;
                    }

                    yield return null;
                }
            }
            else // idle
            {
                IdleAnim();
                NavMeshAgent.SetDestination(transform.position);
                yield return new WaitForSeconds(timePatrol);
            }

            yield return null;
        }
    }

    private IEnumerator Follow()
    {
        yield return null;
        // Debug.Log("Follow" + GetInstanceID().ToString());

        Vector3 lastTargetPosition = Vector3.zero;

        while (true)
        {
            if (NavMeshAgent != null)
            {
                if (_targetFollow != null)
                {
                    Vector3 currentTargetPosition = _targetFollow.transform.position;
                    if (Vector3.Distance(lastTargetPosition, currentTargetPosition) > 1f)
                    {
                        lastTargetPosition = currentTargetPosition;
                        NavMeshAgent.SetDestination(currentTargetPosition);
                        _meshAnimator.speed = NavMeshAgent.velocity.magnitude / 3;
                        RunAnim();
                    }
                }
                else
                {
                    NavMeshAgent.SetDestination(transform.position);
                    if (targetChase == null)
                    {
                        ChangeState(Patrol(), State.Patrol);
                    }
                    else
                    {
                        ChangeState(Chase(), State.Chase);
                    }
                }
            }
            yield return new WaitForSeconds(frameDelay);
        }
    }

    public IEnumerator Chase()
    {
        yield return null;
        // Debug.Log("Chase" + GetInstanceID().ToString());

        Vector3 lastTargetPosition = Vector3.zero;

        while (true)
        {
            if (NavMeshAgent != null)
            {
                Vector3 currentTargetPosition = targetChase.transform.position;
                if (Vector3.Distance(transform.position, currentTargetPosition) > rangeOfChase)
                {
                    targetChase = null;
                    ChangeState(Patrol(), State.Patrol);
                }

                if (Vector3.Distance(lastTargetPosition, currentTargetPosition) > 1f)
                {
                    lastTargetPosition = currentTargetPosition;
                    NavMeshAgent.SetDestination(currentTargetPosition);
                    RunAnim();
                }
            }
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private IEnumerator Attack()
    {
        yield return null;
        // Debug.Log("Attack" + GetInstanceID().ToString());

        AttackAnim();
        float animTime = _meshAnimator.currentAnimation.Length / _meshAnimator.speed;
        while (true)
        {
            if (targetAttack != null)
            {
                _attacking = true;
                yield return new WaitForSeconds(animTime);
                _attacking = false;
            }
            else
            {
                // Animator.speed = 1f;
                if (_targetFollow != null && targetChase == null)
                {
                    ChangeState(Follow(), State.Follow);
                }
                else
                {
                    if (targetChase == null)
                    {
                        ChangeState(Patrol(), State.Patrol);
                    }
                    else
                    {
                        ChangeState(Chase(), State.Chase);
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator Death()
    {
        yield return null;
        DeathAnim();
        float animTime = _meshAnimator.currentAnimation.Length / _meshAnimator.speed;
        yield return new WaitForSeconds(animTime);
        Factory.Instance.enemyFactory.ReleaseEnemy(this);
    }

    public void TargetFollow(BaseCharacter target)
    {
        _targetFollow = target;
        if (_targetFollow != null)
        {
            if (targetAttack != null) return;
            ChangeState(Follow(), State.Follow);
        }
    }

    public override void ApplyAttack(BaseCharacter target)
    {
        base.ApplyAttack(target);
        targetAttack = target;

        if (_curState != State.Attack)
        {
            NavMeshAgent.SetDestination(transform.position);
            ChangeState(Attack(), State.Attack);
        }
    }

    public override void RemoveeAttack()
    {
        base.RemoveeAttack();
        StartCoroutine(WaitRemoveAttack());
    }

    private IEnumerator WaitRemoveAttack()
    {
        yield return null;
        while (_attacking == true)
        {
            yield return null;
        }

        targetAttack = null;
    }

    private void OnAttackEvent()
    {
        SendDamage(targetAttack, this);
    }

    public override void SendDamage(BaseCharacter target, BaseCharacter sender)
    {
        base.SendDamage(target, sender);
    }

    public override void TackDamage(BaseCharacter target, BaseCharacter sender)
    {
        base.TackDamage(target, sender);
        if (isDie) return;

        if (targetChase != null) return;
        targetChase = UserMain.Instance.userArmy;

        if (targetAttack != null) return;
        ChangeState(Chase(), State.Chase);
    }

    public override void Die()
    {
        NavMeshAgent.SetDestination(transform.position);
        AllColliderEnabled(false);

        base.Die();
        StopAllCoroutines();
        ChangeState(Death(), State.Die);

        UserMain.Instance.KillAmount += 1;
        ItemsDrop.Instance.DropItem(transform.position);
    }

    public void IdleAnim()
    {
        if (_meshAnimator.currentAnimation.AnimationName != "Z_idle_A")
        {
            _meshAnimator.speed = 1;
            _meshAnimator.Play("Z_idle_A");
            NavMeshAgent.speed = 0;
        }
    }

    private void RunAnim()
    {
        if (_meshAnimator.currentAnimation.AnimationName != "Z_run")
        {
            _meshAnimator.speed = 1;
            _meshAnimator.Play("Z_run");
            NavMeshAgent.speed = MoveSpeed;
        }
    }

    private void WalkAnim()
    {
        if (_meshAnimator.currentAnimation.AnimationName != "Z_walk")
        {
            _meshAnimator.speed = 1;
            _meshAnimator.Play("Z_walk");
            NavMeshAgent.speed = 1.5f;
        }
    }

    private void AttackAnim()
    {
        if (_meshAnimator.currentAnimation.AnimationName != "Z_attack_A")
        {
            _meshAnimator.speed = 1;
            _meshAnimator.Play("Z_attack_A");
            NavMeshAgent.speed = 0;
        }
    }

    private void DeathAnim()
    {
        if (_meshAnimator.currentAnimation.AnimationName != "Z_death_A")
        {
            _meshAnimator.speed = 1;
            _meshAnimator.Play("Z_death_A");
            NavMeshAgent.speed = 0;
        }
    }
}