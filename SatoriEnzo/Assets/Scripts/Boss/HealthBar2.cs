using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PixelHeartHealth : MonoBehaviour
{
    public float maxHealth = 5;
    public float currentHealth = 5;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public List<Image> hearts;

    public void SetHealth(float hp)
    {
        currentHealth = Mathf.Clamp(hp, 0, maxHealth);
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    private void Start()
    {
        UpdateHearts();
    }

    public void SetMaxValue(float value)
    {
        maxHealth = value;
        currentHealth = value;

    }

}
