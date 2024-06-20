using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBarTween : MonoBehaviour
{
    private Slider _progress;
    private float _duration = 0.3f;

    private void Awake()
    {
        _progress = GetComponent<Slider>();
    }

    public float Value
    {
        get { return _progress.value; }
        set
        {
            var progressStart = _progress.value;
            DOTween.To(
                () => progressStart, x =>
                _progress.value = x,
                value, _duration
            ).SetEase(Ease.OutQuad);
        }
    }
}