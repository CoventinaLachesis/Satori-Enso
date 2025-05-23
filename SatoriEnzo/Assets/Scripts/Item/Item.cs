using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float bonusJumpSpeed = 0;
    public float bonusHorizontalSpeed = 0;
    public int bonusJump = 0;
    public bool bonusShield = false;
    public bool reverseMovement = false;
    public bool spawnPlatform = false;
    public float platformDuration = 0;
    public Vector2 spawnPlatformOffset = new Vector2(0, 0);
    public bool platformDissolveEffect = false;
    public float platformDisableDuration = 7;
    public float bonusScale = 0;
    public float duration = 10;
    public float expireTime = 8;
    public float blinkInterval = 0.2f;
    private float blinkDuration;
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkDuration = expireTime * 0.2f; // 20% of total time
        StartCoroutine(Expire());
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(expireTime - blinkDuration);

        StartCoroutine(BlinkAndDestroy());
    }

    IEnumerator BlinkAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < blinkDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        Destroy(gameObject);
    }
}
