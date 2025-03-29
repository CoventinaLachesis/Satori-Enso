using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float disabledOpacity = 0.1f;
    [SerializeField] private float enabledOpacity = 1f;
    [SerializeField] private Color tempColor;
    private BoxCollider2D platformCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void WaitAndDestroy(float duration)
    {
        Debug.Log(tempColor);
        spriteRenderer.color = tempColor;
        StartCoroutine(DestroyPlatform(duration));
    }

    public void TempDisablePlatform(float duration)
    {
        DisablePlatform();
        StartCoroutine(DelayedEnablePlatform(duration));
    }

    IEnumerator DelayedEnablePlatform(float duration)
    {
        yield return new WaitForSeconds(duration);
        EnablePlatform();
    }

    IEnumerator DestroyPlatform(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void DisablePlatform()
    {
        SetTransparency(disabledOpacity);
        platformCollider.enabled = false;
    }

    public void EnablePlatform()
    {
        SetTransparency(enabledOpacity);
        platformCollider.enabled = true;
    }

    private void SetTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha; // Change alpha value
        spriteRenderer.color = color;
    }
}

    
