using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemsDrop : MonoBehaviour
{
    [SerializeField] private BaseItem _moneyPrefab;
    [SerializeField] private BaseItem _expPrefab;
    [SerializeField] private BaseItem _healthPrefab;

    [Header("Rate Item Drop")]
    [SerializeField] private float _expDrop;
    [SerializeField] private float _moneyDrop;
    [SerializeField] private float _healthDrop;

    [Header("Max Item Drop")]
    [SerializeField] private int _maxHealthDrop;


    private Dictionary<ItemType, ObjectPool<BaseItem>> _itemsPool;

    private static ItemsDrop _instance;
    public static ItemsDrop Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemsDrop>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _itemsPool = new Dictionary<ItemType, ObjectPool<BaseItem>>()
        {
            { ItemType.Money, CreateObjectPool(_moneyPrefab) },
            { ItemType.Exp, CreateObjectPool(_expPrefab) },
            { ItemType.Health, CreateObjectPool(_healthPrefab) }
        };
    }

    private ObjectPool<BaseItem> CreateObjectPool(BaseItem baseItem)
    {
        return new ObjectPool<BaseItem>(
            createFunc: () => CreatePool(baseItem),
            actionOnGet: GetPool,
            actionOnRelease: ReleasePool
        );
    }

    private BaseItem CreatePool(BaseItem baseItem)
    {
        return Instantiate(baseItem, transform);
    }

    private void GetPool(BaseItem item)
    {
        item.gameObject.SetActive(true);
    }

    private void ReleasePool(BaseItem item)
    {
        item.gameObject.SetActive(false);
    }

    public void DropItem(Vector3 position)
    {
        // drop health
        var healthRate = GameUtils.ReverseLinear(UserMain.Instance.userArmy.Health, UserMain.Instance.userArmy.MaxHealth * 0.7f, 0, _healthDrop);
        if (Random.value < healthRate)
        {
            var healthItem = _itemsPool[ItemType.Health].Get();
            healthItem.SetAmount(UserMain.Instance.userArmy);
            var posRan = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            healthItem.transform.position = position + posRan;
        }

        // drop money
        if (Random.value < _moneyDrop)
        {
            var moneyItem = _itemsPool[ItemType.Money].Get();
            moneyItem.SetAmount(UserMain.Instance.Level);
            moneyItem.transform.position = position;
            return;
        }

        // drop exp
        if (Random.value < _expDrop)
        {
            var expItem = _itemsPool[ItemType.Exp].Get();
            expItem.SetAmount(UserMain.Instance.Level);
            expItem.transform.position = position;
            return;
        }
    }

    public void ReleaseItem(BaseItem item)
    {
        _itemsPool[item.Type].Release(item);
    }
}