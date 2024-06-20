using UnityEngine;

public class MoneyItem : BaseItem
{
    [SerializeField] private Vector2Int _baseMoneyDrop = new Vector2Int(1, 5);
    [SerializeField] private Material _materialGolg;
    [SerializeField] private Material _materialSilver;

    private MeshRenderer _meshRenderer;

    public override void SetAmount(int level)
    {
        base.SetAmount(level);

        var randomBase = GameUtils.RandomWeighted(_baseMoneyDrop.x, _baseMoneyDrop.y, 2);
        amount = Mathf.RoundToInt(randomBase * level);

        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (randomBase < (int)_baseMoneyDrop.y * 0.7)
        {
            _meshRenderer.material = _materialSilver;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            _meshRenderer.material = _materialGolg;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}