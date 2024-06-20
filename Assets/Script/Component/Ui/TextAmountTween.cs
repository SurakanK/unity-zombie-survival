using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAmountTween : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private float _duration = 0.3f;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public int Value
    {
        set
        {
            transform.DOScale(Vector3.one * 1.2f, _duration).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InQuad);
                });

            var startNumber = GameUtils.ParseAmount(_text.text);
            DOTween.To(() => startNumber, 
                x => _text.text = GameUtils.AmountFormat(x), 
                value, _duration)
                .SetEase(Ease.Linear);
        }
    }
}