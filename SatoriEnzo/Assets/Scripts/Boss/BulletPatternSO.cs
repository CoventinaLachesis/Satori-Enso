// BulletPatternSO.cs
using System.Collections;
using UnityEngine;

public abstract class BulletPatternSO : ScriptableObject
{
    public string patternName = "Pattern";
    public GameObject bulletPrefab;
    public float speed = 5f;
    public int bulletCount = 12;
    public BulletMotionType motionType = BulletMotionType.Straight;
    public float waveFrequency = 5f;
    public float waveAmplitude = 0.5f;
    [SerializeField] protected AudioClip shootSound;

    public virtual IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        yield return null;
    }

    protected void ShootBullet(Transform firePoint, float angleDeg)
    {
        float spawnRadius = bulletPrefab.GetComponent<Collider2D>()?.bounds.extents.magnitude ?? 0.5f;
        Vector2 direction = Quaternion.Euler(0, 0, angleDeg) * Vector2.right;
        Vector2 spawnPos = (Vector2)firePoint.position + direction * spawnRadius;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BossBullet b = bullet.GetComponent<BossBullet>();
        if (b != null)
        {
            b.motionType = motionType;
            b.waveFrequency = waveFrequency;
            b.waveAmplitude = waveAmplitude;
            b.speed = speed;
            b.SetDirection(direction);
        }
        FXPlayer.PlaySound(shootSound, spawnPos);
    }
    protected void DrawSineWavePath(Vector3 origin, float angleDeg, float waveAmp, float waveFreq, float speed, int samples = 30, float spacing = 0.1f)
    {
        Vector3 dir = Quaternion.Euler(0, 0, angleDeg) * Vector3.right;
        Vector3 side = Quaternion.Euler(0, 0, angleDeg + 90f) * Vector3.right;

        Vector3 lastPoint = origin;

        for (int i = 1; i <= samples; i++)
        {
            float t = i * spacing;
            Vector3 next = origin + dir * (speed * t) + side * Mathf.Sin(t * waveFreq) * waveAmp;
            Gizmos.DrawLine(lastPoint, next);
            lastPoint = next;
        }
    }
    protected float GetAngleFromDirection(ShootDirection direction)
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
#if UNITY_EDITOR
    public virtual void DrawGizmos(Transform firePoint)
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right * 2f;
            Gizmos.DrawRay(firePoint.position, dir);
        }
    }
#endif

}