using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// OrbitingBulletRingPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Orbiting Ring")]
public class OrbitingBulletRingPattern : BulletPatternSO
{
    public float initialRadius = 2f;
    public float expansionRate = 0.5f;
    public float sizeIncreaseRate = 0.2f;
    public float orbitSpeed = 90f;
    public float duration = 5f;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        int count = bulletCount;
        GameObject[] bullets = new GameObject[count];
        float[] angles = new float[count];
        float radius = initialRadius;

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count);
            angles[i] = angle;
            Vector2 spawnPos = firePoint.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0);

            bullets[i] = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            radius += expansionRate * Time.deltaTime;

            for (int i = 0; i < count; i++)
            {
                if (bullets[i] != null)
                {
                    angles[i] += orbitSpeed * Time.deltaTime;
                    Vector2 newPos = firePoint.position + new Vector3(
                        Mathf.Cos(angles[i] * Mathf.Deg2Rad) * radius,
                        Mathf.Sin(angles[i] * Mathf.Deg2Rad) * radius,
                        0);
                    bullets[i].transform.position = newPos;
                    bullets[i].transform.localScale += Vector3.one * sizeIncreaseRate * Time.deltaTime;
                }
            }
            yield return null;
        }

        foreach (var b in bullets)
        {
            if (b != null) Destroy(b);
        }
    }
}
