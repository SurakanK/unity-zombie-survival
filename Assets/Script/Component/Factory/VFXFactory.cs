using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class StatusVFX
{
    public StatusEffectType type;
    public ParticleSystem vfx;
}

[Serializable]
public class CharacterVFX
{
    public VFXType type;
    public ParticleSystem vfx;
}

public class VFXFactory : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private List<StatusVFX> _statusVFX;
    [SerializeField] private List<CharacterVFX> _characterVFX;

    public Dictionary<StatusEffectType, ObjectPool<ParticleSystem>> StatusVFXPool;
    public Dictionary<VFXType, ObjectPool<ParticleSystem>> CharacterVFXPool;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        StatusVFXPool = new Dictionary<StatusEffectType, ObjectPool<ParticleSystem>>();
        CharacterVFXPool = new Dictionary<VFXType, ObjectPool<ParticleSystem>>();

        for (int i = 0; i < _statusVFX.Count; i++)
        {
            var status = _statusVFX[i];
            StatusVFXPool.Add(status.type, CreateObjectPool(status.vfx));
        }

        for (int i = 0; i < _characterVFX.Count; i++)
        {
            var status = _characterVFX[i];
            CharacterVFXPool.Add(status.type, CreateObjectPool(status.vfx));
        }
    }

    private ObjectPool<ParticleSystem> CreateObjectPool(ParticleSystem vfx)
    {
        return new ObjectPool<ParticleSystem>(
            createFunc: () => CreatePool(vfx),
            actionOnGet: GetPool,
            actionOnRelease: ReleasePool
        );
    }

    private ParticleSystem CreatePool(ParticleSystem vfx)
    {
        return Instantiate(vfx, _container.transform);
    }

    private void GetPool(ParticleSystem vfx)
    {
        vfx.gameObject.SetActive(true);
    }

    private void ReleasePool(ParticleSystem vfx)
    {
        vfx.gameObject.SetActive(false);
        vfx.transform.parent = _container.transform;
    }

    public void ReleaseVFX(StatusEffectType type, ParticleSystem vfx)
    {
        StatusVFXPool[type].Release(vfx);
    }

    public void ReleaseVFX(VFXType type, ParticleSystem vfx)
    {
        CharacterVFXPool[type].Release(vfx);
    }
}