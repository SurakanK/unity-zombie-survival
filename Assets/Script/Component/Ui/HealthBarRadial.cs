
using UnityEngine;
using UnityEngine.UI;

public class HealthBarRadial : HealthBar
{
    private Image _fillImgae;

    public override void UpdateHealth(float value, float health, float maxHealth)
    {
        base.UpdateHealth(value, health, maxHealth);

        if (_fillImgae == null)
        {
            _fillImgae = progress.fillRect.transform.GetComponent<Image>();
        }

        var s = GameUtils.Map(value, 0, 1f, 0, 0.35f);
        var color = Color.HSVToRGB(s, 1, 1);
        color.a = 0.5f;
        _fillImgae.color = color;
    }
}