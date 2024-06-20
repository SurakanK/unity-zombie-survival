using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [HideInInspector] public Slider progress;

    public virtual void UpdateHealth(float value, float health, float maxHealth)
    {
        if (progress == null)
        {
            progress = GetComponent<Slider>();
        }

        progress.value = value;
    }
}