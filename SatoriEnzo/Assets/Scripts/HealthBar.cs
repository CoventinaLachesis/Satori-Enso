using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image fillBar;        // The actual health fill image
    [SerializeField] private Image backgroundBar;  // Optional background or delayed damage bar

    [Header("Animation")]
    [SerializeField] private float lerpSpeed = 10f;

    [Header("Color Gradient")]
    [SerializeField] private Gradient healthGradient;

    private float maxValue = 100f;
    private float currentValue = 100f;
    private float targetFill = 1f;

    private void Start()
    {
        UpdateFillColor();
        fillBar.fillAmount = 1f;
        if (backgroundBar != null)
            backgroundBar.fillAmount = 1f;
    }

    private void Update()
    {
        float currentFill = fillBar.fillAmount;
        float newFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * lerpSpeed);
        fillBar.fillAmount = newFill;

        if (backgroundBar != null && backgroundBar.fillAmount > targetFill)
        {
            // Optional: damage delay effect
            backgroundBar.fillAmount = Mathf.Lerp(backgroundBar.fillAmount, targetFill, Time.deltaTime * (lerpSpeed / 2f));
        }

        UpdateFillColor();
    }

    public void SetMaxValue(float value)
    {
        maxValue = value;
        currentValue = value;
        targetFill = 1f;
    }

    public void SetCurrentValue(float value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue);
        targetFill = currentValue / maxValue;
    }

    public float GetCurrentValue() => currentValue;
    public float GetMaxValue() => maxValue;

    private void UpdateFillColor()
    {
        if (healthGradient != null)
            fillBar.color = healthGradient.Evaluate(fillBar.fillAmount);
    }
}
