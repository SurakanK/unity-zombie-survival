
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PoisonsAbilitie : BaseAbilitie
{
    private List<BaseSkill> _skills = new List<BaseSkill>();
    private BaseStatus _debuff;

    public PoisonsAbilitie(BaseCharacter owner, AbilitieData abilitieData) : base(owner, abilitieData)
    {

    }

    public override void Initialized()
    {
        base.Initialized();
        _debuff = BaseStatus.CreateStatus(StatusEffectType.Bleeding, null, abilitieData);
    }

    public override void Apply(BaseCharacter target, Trigger trigger)
    {
        base.Apply(target, trigger);
        owner.StartCoroutine(SpawnPoisons());
    }

    private IEnumerator SpawnPoisons()
    {
        yield return null;
        var action = abilitieData.action;

        while (true)
        {
            ClearSkill();

            for (int i = 0; i < action.spawnAmount; i++)
            {
                SpawnSkill();
            }
            yield return new WaitForSeconds(action.cooldown);
        }
    }

    private void ClearSkill()
    {
        foreach (var skill in _skills)
        {
            Factory.Instance.abilitieFactory.ReleaseSkill(skill);
        }

        _skills.Clear();
    }

    private void SpawnSkill()
    {
        var skill = Factory.Instance.abilitieFactory.SkillsPool[abilitieData.type].Get();
        var position = GameUtils.RandomAroundPosition(owner.transform.position, 4);
        skill.transform.localPosition = position;

        skill.Spawn(_debuff);
        _skills.Add(skill);
    }
}