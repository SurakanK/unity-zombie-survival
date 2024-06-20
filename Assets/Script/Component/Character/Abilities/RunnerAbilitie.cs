
using UnityEngine;

internal class RunnerAbilitie : BaseAbilitie
{
    public RunnerAbilitie(BaseCharacter owner, AbilitieData abilitieData) : base(owner, abilitieData)
    {

    }

    public override void Initialized()
    {
        base.Initialized();
    }

    public override void Apply(BaseCharacter target, Trigger trigger)
    {
        base.Apply(target, trigger);
        var npc = target as EnemyNPC;

        npc.FollowArea.spCollider.radius = 20;
        npc.frameDelay = 0.05f;
        npc.NavMeshAgent.autoBraking = false;
        target.MoveSpeed = abilitieData.action.speed;
    }
}