using System.Collections;
using UnityEngine;

public class BossTypeB1: BossPattern
{
    [SerializeField] private BulletPatternConfig[] bulletPatterns;

    protected override void Start()
    {
        Invoke(nameof(InitializeBoss), 1f);
    }

    private void InitializeBoss()
    {
        if (audioSource == null)
        {
            Debug.LogError($"❌ AudioSource is NULL on {gameObject.name}");
        }

        if (bulletPatterns != null && bulletPatterns.Length > 0)
        {
            StartShooting();
        }
    }

    protected override IEnumerator AttackPattern()
    {
        while (true)
        {
            foreach (var config in bulletPatterns)
            {
                yield return new WaitForSeconds(config.delayBeforeStart);
                yield return ExecutePattern(config);
            }
        }
    }

    private IEnumerator ExecutePattern(BulletPatternConfig config)
    {
        switch (config.patternType)
        {
            case BulletPatternType.SpiralShot:
                return SpiralShot(config);

            case BulletPatternType.RadialBurst:
                return RadialBurst(config);

            case BulletPatternType.WaveAttack:
                return WaveAttack(config);

            case BulletPatternType.SweepingArc:
                return SweepingArc(config);

            case BulletPatternType.BulletRings:
                return BulletRings(config);

            case BulletPatternType.OrbitingBulletRing:
                return OrbitingBulletRing(config);

            default:
                Debug.LogWarning($"Unhandled pattern type: {config.patternType}");
                return null;
        }
    }

    // ───────────────────────────── Patterns ─────────────────────────────

    private IEnumerator SpiralShot(BulletPatternConfig config)
    {
        float angle = 0;
        for (int i = 0; i < config.spirals * 20; i++)
        {
            ShootBullet(config, angle);
            angle += 18f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RadialBurst(BulletPatternConfig config)
    {
        for (int i = 0; i < config.bulletCount; i++)
        {
            float angle = i * (360f / config.bulletCount);
            ShootBullet(config, angle);
        }
        yield return null;
    }

    private IEnumerator WaveAttack(BulletPatternConfig config)
    {
        float baseAngle = GetAngleFromDirection(config.shootDirection);

        for (int i = 0; i < config.bulletCount; i++)
        {
            float angle = baseAngle + Mathf.Sin(Time.time * 5f) * 30f;
            ShootBullet(config, angle);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SweepingArc(BulletPatternConfig config)
    {
        float baseAngle = GetAngleFromDirection(config.shootDirection);
        float sweepRange = 90f;

        for (int i = 0; i < config.bulletCount; i++)
        {
            float angle = baseAngle - (sweepRange / 2) + (i * (sweepRange / config.bulletCount));
            ShootBullet(config, angle);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator BulletRings(BulletPatternConfig config)
    {
        for (int r = 0; r < config.rings; r++)
        {
            for (int i = 0; i < config.bulletCount; i++)
            {
                float angle = i * (360f / config.bulletCount);
                ShootBullet(config, angle);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator OrbitingBulletRing(BulletPatternConfig config)
    {
        int count = config.bulletCount;
        GameObject[] bullets = new GameObject[count];
        float[] angles = new float[count];
        float radius = config.initialRadius;

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count);
            angles[i] = angle;

            Vector2 spawnPos = firePoint.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0);

            bullets[i] = Instantiate(config.bulletPrefab, spawnPos, Quaternion.identity);
        }

        float elapsed = 0f;
        while (elapsed < 5f)
        {
            elapsed += Time.deltaTime;
            radius += config.expansionRate * Time.deltaTime;

            for (int i = 0; i < count; i++)
            {
                if (bullets[i] != null)
                {
                    angles[i] += config.speed * Time.deltaTime;
                    Vector2 newPos = firePoint.position + new Vector3(
                        Mathf.Cos(angles[i] * Mathf.Deg2Rad) * radius,
                        Mathf.Sin(angles[i] * Mathf.Deg2Rad) * radius,
                        0);
                    bullets[i].transform.position = newPos;
                    bullets[i].transform.localScale += Vector3.one * config.sizeIncreaseRate * Time.deltaTime;
                }
            }

            yield return null;
        }

        foreach (var b in bullets)
        {
            if (b != null) Destroy(b);
        }
    }

    // ───────────────────────────── Bullet Spawn ─────────────────────────────

    private void ShootBullet(BulletPatternConfig config, float angleDeg)
    {
        Vector2 dir = Quaternion.Euler(0, 0, angleDeg) * Vector2.right;
        GameObject bullet = Instantiate(config.bulletPrefab, firePoint.position, Quaternion.identity);

        BossBullet b = bullet.GetComponent<BossBullet>();
        b.motionType = config.motionType;
        b.waveFrequency = config.waveFrequency;
        b.waveAmplitude = config.waveAmplitude;
        b.speed = config.speed;
        b.SetDirection(dir);
    }

    // ───────────────────────────── Helpers ─────────────────────────────

    private float GetAngleFromDirection(ShootDirection direction)
    {
        return direction switch
        {
            ShootDirection.Up => 90f,
            ShootDirection.Down => 270f,
            ShootDirection.Left => 180f,
            ShootDirection.Right => 0f,
            ShootDirection.UpLeft => 135f,
            ShootDirection.UpRight => 45f,
            ShootDirection.DownLeft => 225f,
            ShootDirection.DownRight => 315f,
            _ => 0f
        };
    }
}
