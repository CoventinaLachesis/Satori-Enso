using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SweepingArcPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Sweeping Arc")]
public class SweepingArcPattern : BulletPatternSO
{
    public ShootDirection shootDirection = ShootDirection.Down;
    public float sweepRange = 90f;
    public float interval = 0.15f;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        float baseAngle = GetAngleFromDirection(shootDirection);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle - (sweepRange / 2) + (i * (sweepRange / bulletCount));
            ShootBullet(firePoint, angle);
            yield return new WaitForSeconds(interval);
        }
    }



#if UNITY_EDITOR
public override void DrawGizmos(Transform firePoint)
{
    Gizmos.color = Color.yellow;
    float baseAngle = GetAngleFromDirection(shootDirection);

    for (int i = 0; i < bulletCount; i++)
    {
        float angle = baseAngle - (sweepRange / 2f) + (i * (sweepRange / bulletCount));
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