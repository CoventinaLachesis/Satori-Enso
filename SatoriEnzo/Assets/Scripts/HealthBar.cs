using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image bar;
    private float maxValue;
    private float currentValue;
    private Vector3 initScale;

    void Awake()
    {
        bar = GetComponent<Image>();
        initScale = bar.transform.localScale;
    }

    public void SetMaxValue(float value)
    {
        maxValue = value;
    }

    public float GetMaxValue()
    {
        return maxValue;
    }

    public void SetCurrentValue(float value)
    {
        currentValue = value;
        Debug.Log(currentValue);
        bar.transform.localScale = new Vector3(currentValue/maxValue * initScale.x, initScale.y, initScale.z);
    }

    public float getCurrentValue()
    {
        return currentValue;
    }
}
