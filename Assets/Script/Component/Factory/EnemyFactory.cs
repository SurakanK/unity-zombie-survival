using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


[Serializable]
public class EnemyAssetModel
{
    public EnemyType type;
    public EnemyNPC enemyNPC;
    public List<GameObject> prefabs;
}

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private List<EnemyAssetModel> _enemyAssets;

    public UnityAction<EnemyNPC> EventReleaseEnemy;

    private EnemysDataModel _enemysData;
    private Dictionary<EnemyType, ObjectPool<EnemyNPC>> _enemysPool;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var json = Resources.Load<TextAsset>("enemy.data");
        _enemysData = JsonUtility.FromJson<EnemysDataModel>(json.text);

        _enemysPool = new Dictionary<EnemyType, ObjectPool<EnemyNPC>>();

        for (int i = 0; i < _enemyAssets.Count; i++)
        {
            var enemyAsset = _enemyAssets[i];
            _enemysPool.Add(enemyAsset.type, CreateObjectPool(enemyAsset.type));
        }
    }

    private ObjectPool<EnemyNPC> CreateObjectPool(EnemyType enemyType)
    {
        return new ObjectPool<EnemyNPC>(
            createFunc: () => CreatePool(enemyType),
            actionOnGet: GetPool,
            actionOnRelease: ReleasePool
        );
    }

    private void ReleasePool(EnemyNPC enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void GetPool(EnemyNPC enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.AllColliderEnabled(true);
        enemy.StartEnemy();
    }

    private EnemyNPC CreatePool(EnemyType enemyType)
    {
        // enemy data
        var enemyData = GetEnemyData(enemyType);
        var jsonData = JsonUtility.ToJson(enemyData);

        var enemys = _enemyAssets.Find(e => e.type == enemyType);
        var enemyModel = enemys.prefabs[UnityEngine.Random.Range(0, enemys.prefabs.Count)];

        // add base prefab
        EnemyNPC enemy = Instantiate(enemys.enemyNPC);
        enemy.name = enemy.GetInstanceID().ToString();
        enemy.transform.parent = _container.transform;

        GameObject model = Instantiate(enemyModel, new Vector3(0, 0, 0), Quaternion.identity);
        model.transform.parent = enemy.transform;
        enemy.Initialize(Factions.Enemy, jsonData);
        return enemy;
    }

    public BaseCharacter CreateEnemy(Vector3 position, EnemyType enemyType)
    {
        var enemy = _enemysPool[enemyType].Get();
        enemy.transform.position = position;
        return enemy;
    }

    public void ReleaseEnemy(EnemyNPC enemyNPC)
    {
        _enemysPool[enemyNPC.enemyType].Release(enemyNPC);
        EventReleaseEnemy?.Invoke(enemyNPC);
    }

    private EnemyData GetEnemyData(EnemyType enemyType)
    {
        return _enemysData.enemies.Find(e => e.type == enemyType);
    }
}