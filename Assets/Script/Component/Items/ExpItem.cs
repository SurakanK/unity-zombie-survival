using UnityEngine;

public class ExpItem : BaseItem
{
    [SerializeField] private Vector2Int _baseExpDrop = new Vector2Int(5, 10);
    [SerializeField] private Material _materialSmall;
    [SerializeField] private Material _materialBig;

    private MeshRenderer _meshRenderer;

    public override void SetAmount(int level)
    {
        base.SetAmount(level);
        var randomBase = GameUtils.RandomWeighted(_baseExpDrop.x, _baseExpDrop.y, 3);
        amount = Mathf.RoundToInt(randomBase * level);

        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (randomBase < (int)_baseExpDrop.y * 0.9)
        {
            _meshRenderer.material = _materialSmall;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            _meshRenderer.material = _materialBig;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}