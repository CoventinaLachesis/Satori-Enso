using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RadialBurstPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Radial Burst")]
public class RadialBurstPattern : BulletPatternSO
{
    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            ShootBullet(firePoint, angle);
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

