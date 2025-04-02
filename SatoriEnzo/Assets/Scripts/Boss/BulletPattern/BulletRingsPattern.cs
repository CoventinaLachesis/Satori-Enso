using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BulletRingsPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Bullet Rings")]
public class BulletRingsPattern : BulletPatternSO
{
    public int rings = 3;
    public float ringDelay = 0.5f;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        for (int r = 0; r < rings; r++)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * (360f / bulletCount);
                ShootBullet(firePoint, angle);
                Debug.Log($" Ring {r}, Bullet {i}, Angle: {angle}");
            }
            yield return new WaitForSeconds(ringDelay);
        }
        yield return null;
    }
#if UNITY_EDITOR
    public override void DrawGizmos(Transform firePoint)
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            if (motionType == BulletMotionType.SineWave)
            {
                DrawSineWavePath(firePoint.position, angle, waveAmplitude, waveFrequency, speed);
            }
            else
            {
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right * 2f;
                Gizmos.DrawRay(firePoint.position, dir);
            }
        }
    }
#endif
}