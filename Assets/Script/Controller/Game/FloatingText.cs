using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public enum FloatingType { Damage }

public class FloatingText : MonoBehaviour
{
    [SerializeField] private GameObject _floatingText;

    private ObjectPool<GameObject> _poolFloating;

    private static FloatingText _instance;
    public static FloatingText Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FloatingText>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _poolFloating = new ObjectPool<GameObject>(CreatePool, GetPool, ReleasePool);
    }

    private GameObject CreatePool()
    {
        return Instantiate(_floatingText, this.transform);
    }

    private void GetPool(GameObject floating)
    {
        floating.SetActive(true);
    }

    private void ReleasePool(GameObject floating)
    {
        floating.SetActive(false);
    }

    public void FloatingTextDamageArmy(string text, bool isBig, Transform transform)
    {
        var position = TextPosition(transform);
        var fontSize = 6f;
        var color = GameUtils.HexToRGB("#930002");
        StartCoroutine(CreateFloating(text, fontSize, color, position));
    }

    public void FloatingTextString(string text, Transform transform, bool isArmy)
    {
        var position = TextPosition(transform);
        var fontSize = isArmy ? 4f : 4f;
        var color = GameUtils.HexToRGB("#45B9A6");
        StartCoroutine(CreateFloating(text, fontSize, color, position));
    }

    public void FloatingTextString(FloatingType type, string text, bool isBig, Transform transform, bool isArmy)
    {
        var position = TextPosition(transform);
        var fontSize = isArmy ? 6f : 4f;
        var color = Color.white;

        switch (type)
        {
            case FloatingType.Damage:
                if (isBig)
                {
                    color = isArmy ? GameUtils.HexToRGB("#930002") : Color.yellow;
                    fontSize = isArmy ? 7f : 5f;
                }
                else
                {
                    color = isArmy ? GameUtils.HexToRGB("#FF2A00") : Color.white;
                }
                break;
        }

        StartCoroutine(CreateFloating(text, fontSize, color, position));
    }

    private Vector3 TextPosition(Transform transform)
    {
        var position = transform.position;
        var offset = new Vector3(0, 0, -1f);
        var spawnPos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1.45f, 1.75f), 0);
        return position + spawnPos + offset;
    }

    private IEnumerator CreateFloating(string text, float fontSize, Color color, Vector3 position)
    {
        yield return null;
        var textObject = _poolFloating.Get();
        textObject.transform.position = position;

        var tmp = textObject.GetComponentInChildren<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;

        yield return new WaitForSeconds(0.5f);
        _poolFloating.Release(textObject);
    }
}