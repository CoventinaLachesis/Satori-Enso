using System.Collections;
using UnityEngine;

public class BossTypeB : BossPattern
{
    [SerializeField] private BulletPatternConfig[] bulletPatterns;

    protected override void Start()
    {
        Invoke(nameof(InitializeBoss), 1f); // Ensures AudioSource is ready
    }

    private void InitializeBoss()
    {
        if (audioSource == null)
        {
            Debug.LogError("❌ AudioSource is still NULL on " + gameObject.name);
        }

        StartShooting();
    }

    protected override IEnumerator AttackPattern()
    {
        while (true)
        {
            foreach (var config in bulletPatterns)
            {
                yield return new WaitForSeconds(config.delayBeforeStart);
                switch (config.patternType)
                {
                    case BulletPatternType.OrbitingBulletRing:
                        yield return OrbitingBulletRing(
                            config.bulletCount,
                            config.initialRadius,
                            config.expansionRate,
                            config.sizeIncreaseRate,
                            config.speed
                        );
                        break;

                    case BulletPatternType.SpiralShot:
                        yield return SpiralShot(config.spirals);
                        break;

                    case BulletPatternType.RadialBurst:
                        yield return RadialBurst(config.bulletCount, config.speed);
                        break;

                    case BulletPatternType.WaveAttack:
                        yield return WaveAttack(config.bulletCount, config.speed, config.shootDirection);
                        break;

                    case BulletPatternType.SweepingArc:
                        yield return SweepingArc(config.bulletCount, config.speed, config.shootDirection);
                        break;

                    case BulletPatternType.BulletRings:
                        yield return BulletRings(config.rings, config.bulletCount, config.speed);
                        break;
                }
            }
        }
    }

    // 1️⃣ Spiral Shot - Bullets rotate in a spiral pattern
    private IEnumerator SpiralShot(int spirals)
    {
        float angle = 0;
        for (int i = 0; i < spirals * 20; i++) // More bullets = denser spiral
        {
            ShootBullet(angle, 5f);
            angle += 18f; // Rotate slightly each shot
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 2️⃣ Radial Burst - Shoots bullets in all directions
    private IEnumerator RadialBurst(int bulletCount, float speed)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            ShootBullet(angle, speed);
        }
        yield return null; // Instant burst
    }

    // 3️⃣ Wave Attack - Bullets follow a wavy pattern
    private IEnumerator WaveAttack(int bullets, float speed, ShootDirection direction)
    {
        float baseAngle = GetAngleFromDirection(direction);

        for (int i = 0; i < bullets; i++)
        {
            float angle = baseAngle + Mathf.Sin(Time.time * 5f) * 30f; // Wave motion around base angle
            ShootBullet(angle, speed);
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 4️⃣ Sweeping Arc - Bullets sweep from one side to the other
    private IEnumerator SweepingArc(int bulletCount, float speed, ShootDirection direction)
    {
        float baseAngle = GetAngleFromDirection(direction);
        float sweepRange = 90f; // Default sweeping range

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle - (sweepRange / 2) + (i * (sweepRange / bulletCount));
            ShootBullet(angle, speed);
            yield return new WaitForSeconds(0.15f);
        }
    }

    // 5️⃣ Bullet Rings - Fires multiple rings of bullets outward
    private IEnumerator BulletRings(int rings, int bulletsPerRing, float speed)
    {
        for (int r = 0; r < rings; r++)
        {
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * (360f / bulletsPerRing);
                ShootBullet(angle, speed);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    private IEnumerator OrbitingBulletRing(int bulletsPerRing, float initialRadius, float expansionRate, float sizeIncreaseRate, float speed)
    {
        GameObject[] bullets = new GameObject[bulletsPerRing]; // Store bullets for movement
        float[] angles = new float[bulletsPerRing]; // Store angles of each bullet
        float radius = initialRadius; // Start radius

        // Create bullets in a circular formation
        for (int i = 0; i < bulletsPerRing; i++)
        {
            float angle = i * (360f / bulletsPerRing);
            angles[i] = angle;

            // Calculate spawn position around the boss
            Vector2 spawnPos = firePoint.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0);
            bullets[i] = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        }

        // Make bullets orbit and expand over time
        float lifetime = 5f; // How long bullets stay before disappearing
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;
            radius += expansionRate * Time.deltaTime; // Expand bullets outward over time

            for (int i = 0; i < bulletsPerRing; i++)
            {
                if (bullets[i] != null)
                {
                    angles[i] += speed * Time.deltaTime; // Rotate bullets around the boss
                    Vector2 newPos = firePoint.position + new Vector3(Mathf.Cos(angles[i] * Mathf.Deg2Rad) * radius, Mathf.Sin(angles[i] * Mathf.Deg2Rad) * radius, 0);
                    bullets[i].transform.position = newPos;

                    // Gradually increase bullet size
                    bullets[i].transform.localScale += Vector3.one * sizeIncreaseRate * Time.deltaTime;
                }
            }

            yield return null; // Wait for the next frame
        }

        // Destroy bullets after they finish orbiting
        for (int i = 0; i < bulletsPerRing; i++)
        {
            if (bullets[i] != null)
            {
                Destroy(bullets[i]);
            }
        }
    }

    private float GetAngleFromDirection(ShootDirection direction)
    {
        switch (direction)
        {
            case ShootDirection.Up: return 90f;
            case ShootDirection.Down: return 270f;
            case ShootDirection.Left: return 180f;
            case ShootDirection.Right: return 0f;
            case ShootDirection.UpLeft: return 135f;
            case ShootDirection.UpRight: return 45f;
            case ShootDirection.DownLeft: return 225f;
            case ShootDirection.DownRight: return 315f;
            default: return 0f;
        }
    }

}
