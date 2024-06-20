using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class AbilitieSpawn
{
    public AbilitieType type;
    public BaseSkill skill;
}

public class AbilitieFactory : MonoBehaviour
{
    [SerializeField] private List<AbilitieSpawn> _abilitiesSpawn;

    public Dictionary<AbilitieType, ObjectPool<BaseSkill>> SkillsPool;

    private AbilitiesModel _abilitiesData;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var json = Resources.Load<TextAsset>("abilities.data");
        _abilitiesData = JsonUtility.FromJson<AbilitiesModel>(json.text);

        SkillsPool = new Dictionary<AbilitieType, ObjectPool<BaseSkill>>();

        for (int i = 0; i < _abilitiesSpawn.Count; i++)
        {
            var skill = _abilitiesSpawn[i];
            SkillsPool.Add(skill.type, CreateSkillObjectPool(skill));
        }
    }

    private ObjectPool<BaseSkill> CreateSkillObjectPool(AbilitieSpawn skill)
    {
        return new ObjectPool<BaseSkill>(
            createFunc: () => CreateSkillPool(skill),
            actionOnGet: GetSkillPool,
            actionOnRelease: ReleaseSkillPool
        );
    }

    private BaseSkill CreateSkillPool(AbilitieSpawn skill)
    {
        var skillPool = Instantiate(skill.skill);
        skillPool.type = skill.type;
        return skillPool;
    }

    private void GetSkillPool(BaseSkill skill)
    {
        skill.gameObject.SetActive(true);
    }

    private void ReleaseSkillPool(BaseSkill skill)
    {
        skill.gameObject.SetActive(false);
    }

    public void ReleaseSkill(BaseSkill skill)
    {
        SkillsPool[skill.type].Release(skill);
    }

    public void CreateAbilitie(BaseCharacter owner, string[] abilities)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            var type = Enum.Parse<AbilitieType>(abilities[i], true);
            var abilitieData = _abilitiesData.abilitis.Find(e => e.type == type);
            var abilitie = BaseAbilitie.Create(owner, abilitieData);

            if (abilitie != null)
            {
                owner.Abilities.Add(abilitie);
            }
        }
    }
}