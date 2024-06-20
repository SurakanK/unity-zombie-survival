using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarIcon : HealthBar
{
    [SerializeField] private List<Sprite> _sprites;

    private Animator _animator;
    private Image _fillImgae;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void UpdateHealth(float value, float health, float maxHealth)
    {
        base.UpdateHealth(value, health, maxHealth);

        if (_fillImgae == null)
        {
            _fillImgae = progress.fillRect.transform.GetComponent<Image>();
        }

        // set image fill
        if (value == 0)
        {
            _animator.speed = 0;
        }
        else if (value > 0 && value < 0.2)
        {
            _fillImgae.sprite = _sprites[2];
            _animator.speed = 2f;
        }
        else if (value >= 0.2 && value < 0.5)
        {
            _fillImgae.sprite = _sprites[1];
            _animator.speed = 1;
        }
        else if (value >= 0.5)
        {
            _fillImgae.sprite = _sprites[0];
            _animator.speed = 0.5f;
        }

        _text.text = $"{health}/{maxHealth}";
    }
}